using Newtonsoft.Json.Linq;
namespace BuscadorPokemon
{
    public partial class Form1 : Form
    {
        // HttpClient se crea una sola vez (buena práctica)
        private static readonly HttpClient _http = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(nombre))
            {
                lblEstado.Text = "Escribe el nombre de un Pokémon.";
                return;
            }

            // Deshabilitar botón mientras se hace la petición I/O-bound
            btnBuscar.Enabled = false;
            lblEstado.Text = "Buscando...";
            picSprite.Image = null;

            try
            {
                // ── Petición I/O-bound #1: datos del Pokémon ──────────────
                string url = $"https://pokeapi.co/api/v2/pokemon/{nombre}";
                string json = await _http.GetStringAsync(url);
                // await devuelve el control al Form mientras espera.
                // La UI sigue respondiendo durante la espera.

                // Parsear JSON
                var data = JObject.Parse(json);
                int id = data["id"].Value<int>();
                int peso = data["weight"].Value<int>();
                int altura = data["height"].Value<int>();
                string tipo = data["types"][0]["type"]["name"].Value<string>();
                string spriteUrl = data["sprites"]["front_default"].Value<string>();

                // Actualizar etiquetas (estamos en el hilo de UI, está bien)
                lblId.Text = $"ID: #{id}";
                lblTipo.Text = $"Tipo: {tipo}";
                lblPeso.Text = $"Peso: {peso / 10.0} kg";
                lblAltura.Text = $"Altura: {altura / 10.0} m";
                lblEstado.Text = $"✔ {nombre} encontrado";

                // ── Petición I/O-bound #2: descargar el sprite ────────────
                byte[] bytes = await _http.GetByteArrayAsync(spriteUrl);
                using var ms = new MemoryStream(bytes);
                picSprite.Image = Image.FromStream(ms);
            }
            catch (HttpRequestException)
            {
                lblEstado.Text = "❌ No encontrado. Verifica el nombre.";
                LimpiarLabels();
            }
            catch (Exception ex)
            {
                lblEstado.Text = $"Error: {ex.Message}";
            }
            finally
            {
                // finally siempre se ejecuta, error o no
                btnBuscar.Enabled = true;
            }
        }
        private void LimpiarLabels()
        {
            lblId.Text = "ID: —";
            lblTipo.Text = "Tipo: —";
            lblPeso.Text = "Peso: —";
            lblAltura.Text = "Altura: —";
            picSprite.Image = null;
        }

        // También permite buscar presionando Enter en el TextBox
        private void txtNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBuscar.PerformClick();
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
