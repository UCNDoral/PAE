using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;

namespace SistemaFacturacion.Services
{
    /// <summary>
    /// Centraliza la exportacion de reportes para que el formulario solo se encargue de la UI.
    /// La clase no usa librerias externas: genera CSV, XLSX y PDF con APIs propias de .NET.
    /// </summary>
    public static class ReporteExportador
    {
        /// <summary>
        /// Guarda el reporte en el formato indicado por la extension del archivo seleccionado.
        /// Esto permite que el usuario elija PDF, Excel o CSV desde un solo dialogo.
        /// </summary>
        public static void Exportar(DataTable tabla, string rutaArchivo, string titulo, string rangoFechas)
        {
            var extension = Path.GetExtension(rutaArchivo).ToLowerInvariant();

            switch (extension)
            {
                case ".pdf":
                    ExportarPdf(tabla, rutaArchivo, titulo, rangoFechas);
                    break;
                case ".xlsx":
                    ExportarExcel(tabla, rutaArchivo, titulo, rangoFechas);
                    break;
                case ".csv":
                    ExportarCsv(tabla, rutaArchivo);
                    break;
                default:
                    throw new NotSupportedException("Seleccione una ruta con extension .pdf, .xlsx o .csv.");
            }
        }

        /// <summary>
        /// Exporta CSV como formato plano. Se conserva por compatibilidad y por ser util
        /// cuando el reporte debe importarse a otro sistema.
        /// </summary>
        private static void ExportarCsv(DataTable tabla, string rutaArchivo)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Join(",", tabla.Columns.Cast<DataColumn>().Select(c => EscaparCsv(c.ColumnName))));

            foreach (DataRow fila in tabla.Rows)
            {
                var valores = tabla.Columns.Cast<DataColumn>()
                    .Select(c => EscaparCsv(FormatearValor(fila[c])));
                sb.AppendLine(string.Join(",", valores));
            }

            File.WriteAllText(rutaArchivo, sb.ToString(), new UTF8Encoding(true));
        }

        /// <summary>
        /// Genera un archivo Excel real (.xlsx). Un XLSX es un paquete ZIP con varios XML internos;
        /// aqui se crea la estructura minima que Excel necesita para abrir una hoja con datos.
        /// </summary>
        private static void ExportarExcel(DataTable tabla, string rutaArchivo, string titulo, string rangoFechas)
        {
            if (File.Exists(rutaArchivo))
            {
                File.Delete(rutaArchivo);
            }

            using var archivo = ZipFile.Open(rutaArchivo, ZipArchiveMode.Create);

            AgregarEntrada(archivo, "[Content_Types].xml", CrearContentTypes());
            AgregarEntrada(archivo, "_rels/.rels", CrearRelacionesRaiz());
            AgregarEntrada(archivo, "xl/workbook.xml", CrearWorkbook());
            AgregarEntrada(archivo, "xl/_rels/workbook.xml.rels", CrearRelacionesWorkbook());
            AgregarEntrada(archivo, "xl/styles.xml", CrearEstilosExcel());
            AgregarEntrada(archivo, "xl/worksheets/sheet1.xml", CrearHojaExcel(tabla, titulo, rangoFechas));
        }

        /// <summary>
        /// Crea una hoja de Excel usando celdas inlineStr para texto y celdas numericas
        /// para montos/cantidades. Esto evita construir una tabla compartida de cadenas.
        /// </summary>
        private static string CrearHojaExcel(DataTable tabla, string titulo, string rangoFechas)
        {
            XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
            var filas = new List<XElement>();
            var rowIndex = 1;

            filas.Add(new XElement(ns + "row",
                new XAttribute("r", rowIndex),
                CrearCeldaTexto(ns, "A", rowIndex, titulo, 1)));
            rowIndex++;

            filas.Add(new XElement(ns + "row",
                new XAttribute("r", rowIndex),
                CrearCeldaTexto(ns, "A", rowIndex, rangoFechas, 2)));
            rowIndex += 2;

            filas.Add(new XElement(ns + "row",
                new XAttribute("r", rowIndex),
                tabla.Columns.Cast<DataColumn>()
                    .Select((c, i) => CrearCeldaTexto(ns, ColumnaExcel(i + 1), rowIndex, c.ColumnName, 3))));
            rowIndex++;

            foreach (DataRow fila in tabla.Rows)
            {
                var filaExcel = new XElement(ns + "row", new XAttribute("r", rowIndex));

                for (var i = 0; i < tabla.Columns.Count; i++)
                {
                    var columna = ColumnaExcel(i + 1);
                    var valor = fila[i];
                    filaExcel.Add(CrearCeldaExcel(ns, columna, rowIndex, valor));
                }

                filas.Add(filaExcel);
                rowIndex++;
            }

            var anchoColumnas = tabla.Columns.Cast<DataColumn>()
                .Select((_, i) => new XElement(ns + "col",
                    new XAttribute("min", i + 1),
                    new XAttribute("max", i + 1),
                    new XAttribute("width", 18),
                    new XAttribute("customWidth", 1)));

            var documento = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement(ns + "worksheet",
                    new XElement(ns + "cols", anchoColumnas),
                    new XElement(ns + "sheetData", filas)));

            return documento.ToString(SaveOptions.DisableFormatting);
        }

        /// <summary>
        /// Genera un PDF simple en orientacion horizontal. El contenido se divide en paginas
        /// para que los reportes largos no queden cortados.
        /// </summary>
        private static void ExportarPdf(DataTable tabla, string rutaArchivo, string titulo, string rangoFechas)
        {
            const int filasPorPagina = 24;
            const decimal anchoPagina = 792m;
            const decimal altoPagina = 612m;
            const decimal margen = 36m;
            const decimal altoLinea = 18m;

            var paginas = new List<string>();
            var totalPaginas = Math.Max(1, (int)Math.Ceiling(tabla.Rows.Count / (double)filasPorPagina));

            for (var pagina = 0; pagina < totalPaginas; pagina++)
            {
                var inicio = pagina * filasPorPagina;
                var filas = tabla.Rows.Cast<DataRow>().Skip(inicio).Take(filasPorPagina);
                var contenido = new StringBuilder();

                contenido.AppendLine("BT");
                contenido.AppendLine("/F1 16 Tf");
                EscribirTextoPdf(contenido, margen, altoPagina - 42, titulo);
                contenido.AppendLine("/F1 9 Tf");
                EscribirTextoPdf(contenido, margen, altoPagina - 60, rangoFechas);
                EscribirTextoPdf(contenido, anchoPagina - 120, altoPagina - 60, $"Pagina {pagina + 1} de {totalPaginas}");

                var y = altoPagina - 92;
                var anchoColumna = (anchoPagina - (margen * 2)) / Math.Max(1, tabla.Columns.Count);

                contenido.AppendLine("/F1 8 Tf");
                for (var i = 0; i < tabla.Columns.Count; i++)
                {
                    EscribirTextoPdf(contenido, margen + (i * anchoColumna), y, Recortar(tabla.Columns[i].ColumnName, 18));
                }

                y -= altoLinea;
                foreach (var fila in filas)
                {
                    for (var i = 0; i < tabla.Columns.Count; i++)
                    {
                        EscribirTextoPdf(contenido, margen + (i * anchoColumna), y, Recortar(FormatearValor(fila[i]), 18));
                    }

                    y -= altoLinea;
                }

                contenido.AppendLine("ET");
                paginas.Add(contenido.ToString());
            }

            EscribirArchivoPdf(rutaArchivo, paginas, anchoPagina, altoPagina);
        }

        /// <summary>
        /// Escribe el archivo PDF manualmente: objetos, paginas, fuentes, contenido y tabla xref.
        /// Es suficiente para reportes tabulares y evita dependencias adicionales.
        /// </summary>
        private static void EscribirArchivoPdf(string rutaArchivo, List<string> contenidos, decimal anchoPagina, decimal altoPagina)
        {
            var objetos = new List<string>();
            var cantidadPaginas = contenidos.Count;
            var pagesObjectId = 2;
            var fontObjectId = 3;
            var primerPageId = 4;
            var idsPaginas = Enumerable.Range(0, cantidadPaginas).Select(i => primerPageId + (i * 2)).ToList();

            objetos.Add("<< /Type /Catalog /Pages 2 0 R >>");
            objetos.Add($"<< /Type /Pages /Kids [{string.Join(" ", idsPaginas.Select(id => $"{id} 0 R"))}] /Count {cantidadPaginas} >>");
            objetos.Add("<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica /Encoding /WinAnsiEncoding >>");

            for (var i = 0; i < cantidadPaginas; i++)
            {
                var pageId = primerPageId + (i * 2);
                var contentId = pageId + 1;
                var bytesContenido = Encoding.Latin1.GetBytes(contenidos[i]);

                objetos.Add($"<< /Type /Page /Parent {pagesObjectId} 0 R /MediaBox [0 0 {NumeroPdf(anchoPagina)} {NumeroPdf(altoPagina)}] /Resources << /Font << /F1 {fontObjectId} 0 R >> >> /Contents {contentId} 0 R >>");
                objetos.Add($"<< /Length {bytesContenido.Length} >>\nstream\n{contenidos[i]}endstream");
            }

            using var stream = new FileStream(rutaArchivo, FileMode.Create, FileAccess.Write);
            using var writer = new StreamWriter(stream, Encoding.Latin1);
            var offsets = new List<long> { 0 };

            writer.Write("%PDF-1.4\n");
            writer.Flush();

            for (var i = 0; i < objetos.Count; i++)
            {
                offsets.Add(stream.Position);
                writer.Write($"{i + 1} 0 obj\n{objetos[i]}\nendobj\n");
                writer.Flush();
            }

            var inicioXref = stream.Position;
            writer.Write($"xref\n0 {objetos.Count + 1}\n");
            writer.Write("0000000000 65535 f \n");

            foreach (var offset in offsets.Skip(1))
            {
                writer.Write($"{offset:0000000000} 00000 n \n");
            }

            writer.Write($"trailer\n<< /Size {objetos.Count + 1} /Root 1 0 R >>\nstartxref\n{inicioXref}\n%%EOF");
        }

        private static XElement CrearCeldaExcel(XNamespace ns, string columna, int fila, object valor)
        {
            if (valor == DBNull.Value)
            {
                return CrearCeldaTexto(ns, columna, fila, string.Empty);
            }

            if (valor is int or long or short or byte or decimal or double or float)
            {
                return new XElement(ns + "c",
                    new XAttribute("r", $"{columna}{fila}"),
                    new XElement(ns + "v", Convert.ToString(valor, CultureInfo.InvariantCulture)));
            }

            return CrearCeldaTexto(ns, columna, fila, FormatearValor(valor));
        }

        private static XElement CrearCeldaTexto(XNamespace ns, string columna, int fila, string valor, int estilo = 0)
        {
            var celda = new XElement(ns + "c",
                new XAttribute("r", $"{columna}{fila}"),
                new XAttribute("t", "inlineStr"),
                new XElement(ns + "is", new XElement(ns + "t", valor)));

            if (estilo > 0)
            {
                celda.Add(new XAttribute("s", estilo));
            }

            return celda;
        }

        private static void AgregarEntrada(ZipArchive archivo, string rutaInterna, string contenido)
        {
            var entrada = archivo.CreateEntry(rutaInterna, CompressionLevel.Fastest);
            using var writer = new StreamWriter(entrada.Open(), Encoding.UTF8);
            writer.Write(contenido);
        }

        private static string CrearContentTypes() =>
            """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types"><Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/><Default Extension="xml" ContentType="application/xml"/><Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/><Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/><Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/></Types>""";

        private static string CrearRelacionesRaiz() =>
            """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/></Relationships>""";

        private static string CrearWorkbook() =>
            """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships"><sheets><sheet name="Reporte" sheetId="1" r:id="rId1"/></sheets></workbook>""";

        private static string CrearRelacionesWorkbook() =>
            """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"><Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/><Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/></Relationships>""";

        private static string CrearEstilosExcel() =>
            """<?xml version="1.0" encoding="UTF-8" standalone="yes"?><styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main"><fonts count="3"><font><sz val="11"/><name val="Calibri"/></font><font><b/><sz val="14"/><name val="Calibri"/></font><font><i/><sz val="10"/><name val="Calibri"/></font></fonts><fills count="2"><fill><patternFill patternType="none"/></fill><fill><patternFill patternType="gray125"/></fill></fills><borders count="1"><border><left/><right/><top/><bottom/><diagonal/></border></borders><cellStyleXfs count="1"><xf numFmtId="0" fontId="0" fillId="0" borderId="0"/></cellStyleXfs><cellXfs count="4"><xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/><xf numFmtId="0" fontId="1" fillId="0" borderId="0" xfId="0"/><xf numFmtId="0" fontId="2" fillId="0" borderId="0" xfId="0"/><xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0" applyFont="1"/></cellXfs></styleSheet>""";

        private static string ColumnaExcel(int indice)
        {
            var nombre = string.Empty;
            while (indice > 0)
            {
                indice--;
                nombre = (char)('A' + (indice % 26)) + nombre;
                indice /= 26;
            }

            return nombre;
        }

        private static void EscribirTextoPdf(StringBuilder contenido, decimal x, decimal y, string texto)
        {
            contenido.AppendLine($"1 0 0 1 {NumeroPdf(x)} {NumeroPdf(y)} Tm ({EscaparPdf(texto)}) Tj");
        }

        private static string NumeroPdf(decimal valor) => valor.ToString("0.##", CultureInfo.InvariantCulture);

        private static string EscaparPdf(string texto) =>
            texto.Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");

        private static string Recortar(string texto, int maximo) =>
            texto.Length <= maximo ? texto : texto[..Math.Max(0, maximo - 3)] + "...";

        private static string FormatearValor(object valor)
        {
            return valor switch
            {
                DBNull => string.Empty,
                DateTime fecha => fecha.ToString("dd/MM/yyyy HH:mm"),
                decimal monto => monto.ToString("0.00"),
                double numero => numero.ToString("0.00"),
                float numero => numero.ToString("0.00"),
                _ => valor.ToString() ?? string.Empty
            };
        }

        private static string EscaparCsv(string valor)
        {
            var texto = valor.Replace("\"", "\"\"");
            return texto.Contains(',') || texto.Contains('"') || texto.Contains('\n')
                ? $"\"{texto}\""
                : texto;
        }
    }
}
