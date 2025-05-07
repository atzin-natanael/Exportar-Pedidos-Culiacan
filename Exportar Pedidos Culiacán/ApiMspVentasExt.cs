using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ApisMicrosip
{
    public class ApiMspVentasExt
    {
        //  function veGetLastErrorCode: Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int veGetLastErrorCode();
        
        //  function veGetLastErrorMessage(ErrorMessage: PChar): Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int veGetLastErrorMessage(StringBuilder ErrorMessage);
        
        //  procedure veSetErrorHandling(ExceptionOnError, MessageOnException: Integer); stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern void veSetErrorHandling(int ExceptionOnError, int MessageOnException);
      
      
        //  // Funciones COMUNES
        //  function SetDBVentas(DBHandle: Integer): Integer;  stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int SetDBVentas(int DBHandle);

        //  procedure AbortaDoctoVentas; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern void AbortaDoctoVentas();

        // function GetDoctoVeId(var DoctoId: Integer): Integer;  stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int GetDoctoVeId(ref int DoctoId);

        //  // Checar Compatibilidad
        //  function GetVersionApiVentasAsInteger: Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int GetVersionApiVentasAsInteger();

        //  procedure GetVersionApiVentasAsString(AVersion: PChar); stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern void GetVersionApiVentasAsString(StringBuilder AVersion);

        //  function ChecaCompatibilidadVentas(HdbMetadatos: Integer): Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int ChecaCompatibilidadVentas(int HdbMetadatos);

        //  // *** Funciones principales PEDIDOS ***
        //  function NuevoPedido(Fecha, Folio: PChar; ClienteId, DirConsigId, AlmacenId: Integer;
        //    FechaEntrega, TipoDscto: PChar; Descuento: Double ; OrdenCompra, Descripcion: Pchar;
        //    VendedorId, ImptoSustituidoId, ImptoSustitutoId: Integer ): Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern void SetReglasPedidos(int AComprometerUnidades, int APedidosParciales,
            int APartidasParciales);

        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int NuevoPedido(string Fecha, string Folio, int ClienteId, int DirConsigId, int AlmacenId,
          string FechaEntrega, string TipoDscto, Double  Descuento, string OrdenCompra, string  Descripcion,
          int VendedorId, int ImptoSustituidoId, int ImptoSustitutoId, int MonedaId);
        
        //  function RenglonPedido(ArticuloId: Integer; Unidades, PrecioUnitario,
        //    PctjeDscto: Double ; Notas: Pchar): Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int RenglonPedido(int ArticuloId, Double  Unidades, 
          Double  PrecioUnitario, Double  PctjeDscto, String Notas);


        // function RenglonPedidoDesctos(ArticuloId: Integer; Unidades, PrecioUnitario,
        //  PctjeDsctoCli, PctjeDsctoVol, PctjeDsctoPromo: Double; Notas: Pchar): Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int RenglonPedidoDesctos(int ArticuloId, Double Unidades,
          Double PrecioUnitario, Double PctjeDsctoCli, Double PctjeDsctoVol, Double PctjeDsctoPromo, String Notas);    
        
        //  function AplicaPedido: Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int AplicaPedido();

        //  // *** Funciones principales FACTURAS ***
        //  procedure SetReglasVentas(ExistenciasNegativas, PrecioMinimo: Integer); stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern void SetReglasVentas(int ExistenciasNegativas, int PrecioMinimo);

        //  function NuevaFactura(Fecha, Folio: PChar;
        //                        ClienteId, DirConsigId, AlmacenId: Integer;
        //                        TipoDscto: Pchar; Descuento: Double;
        //                        OrdenCompra, Descripcion: PChar;
        //                        Fletes, OtrosCargos, PctjeComis : Double;
        //                        CondPagoId, VendedorId,  ImptoSustituidoId, ImptoSustitutoId: Integer;
        //                        ImporteCobro: Double; DescripcionCobro: PChar; PLHandle: Integer): Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int NuevaFactura(string Fecha, string Folio,
                                int ClienteId, int DirConsigId, int AlmacenId,
                                string TipoDscto, Double Descuento,
                                string OrdenCompra, string Descripcion,
                                Double Fletes, Double OtrosCargos, Double PctjeComis,
                                int CondPagoId, int VendedorId, int ImptoSustituidoId, int ImptoSustitutoId,
								Double ImporteCobro, string DescripcionCobro, int PLHandle);

		//  function DirClienteFactura(DirCliId: Integer): Integer; stdcall;
		[DllImport("ApiMicrosip.dll", SetLastError = true)]
		public static extern int DirClienteFactura(int DirCliId);

		// function SetFormaCobroFactura(FormaCobroId: Integer; NumCtaPago: PChar): Integer; stdcall;
		[DllImport("ApiMicrosip.dll", SetLastError = true)]
		public static extern int SetFormaCobroFactura(int FormaCobroId, string NumCtaPago);

		//  function RenglonFactura(ArticuloId: Integer; Unidades, PrecioUnitario,
        //                          PctjeDscto: Double ; Notas: Pchar): Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int RenglonFactura(int ArticuloId, Double Unidades, Double PrecioUnitario,
                                 Double PctjeDscto, string Notas);

        //  function RenglonFacturaSeries(ClaveSerie: PChar): Integer; stdcall;
		[DllImport("ApiMicrosip.dll", SetLastError = true)]
		public static extern int RenglonFacturaSeries(string ClaveSerie);

        //  function RenglonFacturaLotes(ClaveLote: PChar; Unidades: Double ): Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int RenglonFacturaLotes(string ClaveLote, Double Unidades);

        //  function RenglonFacturaTercero(TerceroId: Integer): Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int RenglonFacturaTercero(int TerceroId);

        //  function AplicaFactura: Integer; stdcall;
        [DllImport("ApiMicrosip.dll", SetLastError = true)]
        public static extern int AplicaFactura();
    }

}
