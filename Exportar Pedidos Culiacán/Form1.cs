using System.Text;
using System.Windows.Forms;
using ApisMicrosip;
using FirebirdSql.Data.FirebirdClient;
using LiteDB;
using ApiBas = ApisMicrosip.ApiMspBasicaExt;
using ApiVe = ApisMicrosip.ApiMspVentasExt;

namespace Exportar_Pedidos_Culiacán
{
    public partial class Form1 : Form
    {
        List<List<object>> ArticulosCargados = new List<List<object>>();
        public Form1()
        {
            InitializeComponent();
            LoadConfig();
        }
        public void LoadConfig()
        {
            string filePath = GlobalSettings.Instance.PathConfig + "DB.txt"; // Ruta de tu archivo de texto
            List<string> lineas = new List<string>();

            // Verificar si el archivo existe
            if (File.Exists(filePath))
            {
                // Leer todas las líneas del archivo
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string linea;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        GlobalSettings.Instance.Config.Add(linea);
                    }

                }
                GlobalSettings.Instance.Ip = GlobalSettings.Instance.Config[0];
                GlobalSettings.Instance.Puerto = GlobalSettings.Instance.Config[1];
                GlobalSettings.Instance.Direccion = GlobalSettings.Instance.Config[2];
                GlobalSettings.Instance.User = GlobalSettings.Instance.Config[3];
                GlobalSettings.Instance.Pw = GlobalSettings.Instance.Config[4];

                GlobalSettings.Instance.StringConnection =
                    "User=" + GlobalSettings.Instance.User + ";" +
                    "Password=" + GlobalSettings.Instance.Pw + ";" +
                    "Database=" + GlobalSettings.Instance.Direccion + ";" +
                    "DataSource=" + GlobalSettings.Instance.Ip + ";" +
                    "Port=" + GlobalSettings.Instance.Puerto + ";" +
                    "Dialect=3;" +
                    "Charset=UTF8;";
            }
            else
            {
                MessageBox.Show("Archivo de Configuracion no encontrado", "DB.txt", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void Buscar_Click(object sender, EventArgs e)
        {
            GetFireBirdValue getFireBirdValue = new GetFireBirdValue();
            //string query = "SELECT FOLIO, CLAVE_CLIENTE, VIA_EMBARQUE_ID, IMPORTE_NETO, CLIENTE_ID, DOCTO_VE_ID, DESCRIPCION FROM DOCTOS_VE WHERE (VIA_EMBARQUE_ID = '1268414' OR VIA_EMBARQUE_ID = '1268435') AND TIPO_DOCTO = 'P' AND ALMACEN_ID = '108401' AND ESTATUS = 'P' AND FECHA >= CURRENT_DATE - 1;";
            string query = "SELECT  FOLIO,   CLAVE_CLIENTE,   VIA_EMBARQUE_ID,  IMPORTE_NETO,   CLIENTE_ID,   DOCTO_VE_ID,   DESCRIPCION FROM DOCTOS_VE WHERE   FOLIO IN (    'P00071680', 'P00071681', 'P00071682', 'P00071683', 'P00071679',    'P00071672', 'P00071673', 'P00071684', 'P00071685', 'P00071654',    'P00071653', 'P00071564', 'P00071591', 'P00071652', 'P00071953',    'P00071903', 'P00071904', 'P00071905', 'P00071970', 'P00071971',    'P00071972', 'P00071973', 'P00071967', 'P00071968', 'P00071974',    'P00071969', 'P00071906', 'P00071907', 'P00071865', 'P00071902',    'P00071923', 'P00072052', 'P00071997', 'P00072149'  );";
            List<List<string>> list = getFireBirdValue.GetList(query);
            foreach (var valores in list)
            {
                foreach (var valor in valores)
                {
                    string nombre = getFireBirdValue.GetValue("SELECT NOMBRE FROM CLIENTES WHERE CLIENTE_ID = '" + valores[5] + "';");
                    Tabla.Rows.Add(valores[0], valores[1], valores[2], nombre, valores[6]);
                    query = "SELECT CLAVE_ARTICULO, UNIDADES, PRECIO_UNITARIO, PCTJE_DSCTO,PCTJE_DSCTO_CLI, PRECIO_TOTAL_NETO, UNIDADES_A_SURTIR FROM DOCTOS_VE_DET WHERE DOCTO_VE_ID = '" + valores[6] + "';";
                    List<List<string>> listart = getFireBirdValue.GetList(query);
                    Guardar(listart, "C:\\ConfigDB\\" + valores[6], valores[2], valores[7]);
                    break;
                }
            }
        }
        public class Articulo
        {
            public string Id { get; set; }  // El identificador único (automático si no se especifica)
            public string Clave { get; set; }
            public string Nombre { get; set; }
            public string Precio { get; set; }
            public string Cantidad { get; set; }
            public string Articulo_Id { get; set; }
            public string Nota { get; set; }

            public string Cliente { get; set; }


        }
        public void Guardar(List<List<string>> Articulos, string docto_ve_id, string cliente, string nota)
        {

            using (var db = new LiteDatabase(docto_ve_id))
            {
                // Obtiene la colección 'ARTICULOS' (la tabla en LiteDB)
                var articulosCollection = db.GetCollection<Articulo>("ARTICULOS");
                articulosCollection.DeleteAll();

                // Itera sobre cada lista de artículos y los guarda en la colección
                foreach (var articulo in Articulos)
                {
                    var nuevoArticulo = new Articulo
                    {
                        Id = articulo[0],
                        Clave = articulo[1],
                        Nombre = articulo[2],
                        Precio = articulo[3],
                        Cantidad = articulo[4],
                        Cliente = cliente,
                        Nota = nota
                    };

                    // Insertar el artículo en la colección
                    articulosCollection.Insert(nuevoArticulo);
                }

            }

        }
        private int ConectaBD()
        {

            ApiBas.SetErrorHandling(0, 0);
            if (GlobalSettings.Instance.Bd == 0)
                GlobalSettings.Instance.Bd = ApiBas.NewDB();
            //Objeto transaccion
            //TrnHandle
            GlobalSettings.Instance.Trn = ApiBas.NewTrn(GlobalSettings.Instance.Bd, 3);
            //string path = "192.168.1.4:C:\\Microsip datos\\HIDALGO.fdb"; HIDALGO
            //string path = "192.168.100.8:C:\\Microsip datos\\CULIACAN.fdb";
            string path = GlobalSettings.Instance.Ip + ":" + GlobalSettings.Instance.Direccion;
            //string path = "C:\\Microsip datos\\PAPELERIA CORIBA CORNEJO.fdb";

            //int conecta = ApiBas.DBConnect(GlobalSettings.Instance.Bd, path, "SYSDBA", "C0r1b418"); //HIDALGO
            //int conecta = ApiBas.DBConnect(GlobalSettings.Instance.Bd, path, "SYSDBA", "masterkey");
            int conecta = ApiBas.DBConnect(GlobalSettings.Instance.Bd, path, GlobalSettings.Instance.User, GlobalSettings.Instance.Pw);
            //int conecta = ApiBas.DBConnect(GlobalSettings.Instance.Bd, path, "SYSDBA", "masterkey");
            StringBuilder obtieneError = new StringBuilder(1000);
            int codigoError = ApiBas.GetLastErrorMessage(obtieneError);
            String mensajeError = codigoError.ToString();
            if (codigoError > 0)
            {
                MessageBox.Show(obtieneError.ToString());
                return 0;
            }
            else
            {
                //btnFacturar.Enabled = true;
                return 1;
            }

        }
        public int Art_Id(string clave_principal)
        {
            FbConnection con = new FbConnection("User=" + GlobalSettings.Instance.User + ";" + "Password=" + GlobalSettings.Instance.Pw + ";" + "Database=" + GlobalSettings.Instance.Direccion + ";" + "DataSource=" + GlobalSettings.Instance.Ip + ";" + "Port=" + GlobalSettings.Instance.Puerto + ";" + "Dialect=3;" + "Charset=UTF8;");
            try
            {
                con.Open();
                string query = "SELECT ARTICULO_ID FROM CLAVES_ARTICULOS WHERE CLAVE_ARTICULO = '" + clave_principal + "';";
                FbCommand command = new FbCommand(query, con);
                FbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int art_id = reader.GetInt32(0);
                    return art_id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se perdió la conexión :( , contacta a 06 o intenta de nuevo", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.ToString());
                return 0;
            }
            finally
            {
                con.Close();
            }

        }
        public int Cliente_Id(string clave_principal)
        {
            FbConnection con = new FbConnection("User=" + GlobalSettings.Instance.User + ";" + "Password=" + GlobalSettings.Instance.Pw + ";" + "Database=" + GlobalSettings.Instance.Direccion + ";" + "DataSource=" + GlobalSettings.Instance.Ip + ";" + "Port=" + GlobalSettings.Instance.Puerto + ";" + "Dialect=3;" + "Charset=UTF8;");
            try
            {
                con.Open();
                string query = "SELECT CLIENTE_ID FROM CLAVES_CLIENTES WHERE CLAVE_CLIENTE = '" + clave_principal + "';";
                FbCommand command = new FbCommand(query, con);
                FbDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int art_id = reader.GetInt32(0);
                    return art_id;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se perdió la conexión :( , contacta a 06 o intenta de nuevo", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.ToString());
                return 0;
            }
            finally
            {
                con.Close();
            }

        }
        public void Entrada(string archivoLiteDB)
        {
            ArticulosCargados.Clear();
            using (var db = new LiteDatabase(archivoLiteDB))
            {
                var articulosCollection = db.GetCollection<Articulo>("ARTICULOS");
                var articulos = articulosCollection.FindAll();

                foreach (var articulo in articulos)
                {
                    List<object> articuloLista = new List<object>
            {
                articulo.Id,
                articulo.Clave,
                articulo.Nombre,
                articulo.Precio,
                articulo.Cantidad,
                articulo.Nota,
                articulo.Cliente
            };
                    ArticulosCargados.Add(articuloLista);
                }
            }

            int conecta = ConectaBD();
            if (conecta == 1)
            {
                ApiBas.DBConnected(GlobalSettings.Instance.Bd);
                ApiVe.SetDBVentas(GlobalSettings.Instance.Bd);
                DateTime fecha = DateTime.Now;
                int Cliente = Cliente_Id(ArticulosCargados[0][6].ToString());

                int DoctoId = ApiMspVentasExt.NuevoPedido(fecha.ToString(), "CP", Cliente, 0, 108405, "", "P", 0, "", ArticulosCargados[0][5].ToString(), 813020, 0, 0, 1);
                for (int i = 0; i < ArticulosCargados.Count; ++i)
                {
                    int articulo_id = Art_Id(ArticulosCargados[i][1].ToString());
                    int ped = ApiMspVentasExt.RenglonPedido(articulo_id, double.Parse(ArticulosCargados[i][2].ToString()), double.Parse(ArticulosCargados[i][3].ToString()), double.Parse(ArticulosCargados[i][4].ToString()), "");
                }

                ApiMspVentasExt.GetDoctoVeId(ref DoctoId);
                //MessageBox.Show(DoctoId.ToString());
                int final = ApiVe.AplicaPedido();
                ApiBas.DBDisconnect(GlobalSettings.Instance.Bd);

            }
        }

        //public void Entrada()
        //{
        //    using (var db = new LiteDatabase(openFileDialog1.FileName))
        //    {
        //        // Obtiene la colección 'ARTICULOS' (la tabla en LiteDB)
        //        var articulosCollection = db.GetCollection<Articulo>("ARTICULOS");

        //        // Lista donde vamos a almacenar los artículos en formato lista de listas

        //        // Obtiene todos los artículos de la colección

        //        var articulos = articulosCollection.FindAll();
        //        // Itera sobre cada artículo en la colección
        //        foreach (var articulo in articulos)
        //        {
        //            List<object> articuloLista = new List<object>
        //            // Crea una lista para cada artículo
        //            {
        //                articulo.Id,
        //                articulo.Clave,
        //                articulo.Nombre,
        //                articulo.Precio,
        //                articulo.Cantidad,
        //                articulo.Nota,
        //                articulo.Cliente
        //            };
        //            ArticulosCargados.Add(articuloLista);
        //        }
        //        // Ahora tienes todos los artículos en listaDeArticulos
        //    }
        //    int conecta = ConectaBD();
        //    if (conecta == 1)
        //    {
        //        ApiBas.DBConnected(GlobalSettings.Instance.Bd);
        //        ApiVe.SetDBVentas(GlobalSettings.Instance.Bd);
        //        DateTime fecha = DateTime.Now;
        //        int Cliente= Cliente_Id(ArticulosCargados[0][6].ToString());
        //        //int ErrorFolio = ApiVe.NuevoPedido(Fecha, "IP", int.Parse(Cliente_Id), Dir_Consig_Id, Almacen_Id,"", Tipo_Desc,Descuento,"",Descripcion,Vendedor_Id, 0, 0, Moneda_Id);
        //        int DoctoId = ApiMspVentasExt.NuevoPedido(fecha.ToString(), "CP", Cliente, 0, 108405, "", "P", 0, "", ArticulosCargados[0][5].ToString(), 813020, 0, 0, 1);
        //        for (int i = 0; i < ArticulosCargados.Count; ++i)
        //        {
        //            int articulo_id = Art_Id(ArticulosCargados[i][1].ToString());
        //            int ped = ApiMspVentasExt.RenglonPedido(articulo_id, double.Parse(ArticulosCargados[i][2].ToString()), double.Parse(ArticulosCargados[i][3].ToString()), double.Parse(ArticulosCargados[i][4].ToString()), "");
        //        }
        //        ApiMspVentasExt.GetDoctoVeId(ref DoctoId);
        //        MessageBox.Show(DoctoId.ToString());
        //        int final = ApiVe.AplicaPedido();
                
        //    }

        //}
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Seleccione uno o más archivos";
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string archivo in openFileDialog1.FileNames)
                {
                    GlobalSettings.Instance.nameFile = Path.GetFileName(archivo);
                    Entrada(archivo);
                }
            }
        }
    }
}
