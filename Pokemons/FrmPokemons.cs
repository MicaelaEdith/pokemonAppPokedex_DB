using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Pokemons
{
    public partial class FrmPokemons : Form
    {
        private List<Pokemon> listaPokemon;

        public FrmPokemons()
        {
            InitializeComponent();
        }

        private void FrmPokemons_Load(object sender, EventArgs e)
        {
            cargar();
            cbxCampo.Items.Add("Número");
            cbxCampo.Items.Add("Nombre");
            cbxCampo.Items.Add("Descripción");

        }

        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPokemons.CurrentRow != null)
            {
            Pokemon seleccionado= (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.UrlImagen);

            }
        }

        private void cargar()
        {

            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                listaPokemon = negocio.listar();
                dgvPokemons.DataSource = listaPokemon;
                ocultarColumnas();
                cargarImagen(listaPokemon[0].UrlImagen);
                pBoxPokemon.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void ocultarColumnas()
        {
            dgvPokemons.Columns["UrlImagen"].Visible = false;
            dgvPokemons.Columns["Id"].Visible = false;
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                 pBoxPokemon.Load(imagen);
            }
            catch (Exception ex)
            {

                pBoxPokemon.Load("https://plantillasdememes.com/img/plantillas/imagen-no-disponible01601774755.jpg");
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmAltaPokemon alta = new FrmAltaPokemon();
            alta.ShowDialog();
            cargar();

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (!(dgvPokemons.CurrentRow == null))
            {
                Pokemon seleccionado;
                seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;

                FrmAltaPokemon modificar = new FrmAltaPokemon(seleccionado);
                modificar.ShowDialog();
                cargar();
            }
            else MessageBox.Show("Seleccione un Pokemon para modificar");
        }

        private void btnEliminarF_Click(object sender, EventArgs e)
        {
            if (!(dgvPokemons.CurrentRow == null))
            {
                Eliminar();
            }
            else MessageBox.Show("Seleccione un Pokemon");
        }

        private void btnEliminarL_Click(object sender, EventArgs e)
        {
            if (!(dgvPokemons.CurrentRow == null))
            {
                Eliminar();
            }
            else MessageBox.Show("Seleccione un Pokemon");
        }

        private void Eliminar(bool logico = false)
        {
                PokemonNegocio negocio = new PokemonNegocio();
                Pokemon seleccionado;

                try
                {
                    DialogResult respuesta = MessageBox.Show("¿Desea eliminar el Pokemon seleccionado?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (respuesta == DialogResult.Yes)
                    {
                        seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                    if (logico)
                        negocio.EliminarLogico(seleccionado.Id);
                    else
                        negocio.Eliminar(seleccionado.Id);

                        cargar();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }

        }

        private bool validarFiltro()
        {
            if (cbxCampo.SelectedIndex < 0 || cbxCriterio.SelectedIndex <0)
            {
                MessageBox.Show("Por favor seleccione campo y criterio de búsqueda.");
                return true;
            }
            if (cbxCampo.SelectedIndex==0)
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Por favor ingrese números para filtrar.");
                    return true;
                }
                if (!(soloNumero(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Por favor ingrese solo números.");
                    return true;
                }

            }

            return false;
        }

        private bool soloNumero(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }

            return true;
        }

        private void btnFiltro_Click(object sender, EventArgs e)
        {
            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                if (validarFiltro())
                    return;

                string campo = cbxCampo.SelectedItem.ToString();
                string criterio = cbxCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;
                dgvPokemons.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
            List<Pokemon> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro != "")
            {
                listaFiltrada = listaPokemon.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()));
            }
            else
            {
                listaFiltrada = listaPokemon;
            }
            dgvPokemons.DataSource = null;
            dgvPokemons.DataSource = listaFiltrada;
            ocultarColumnas();

        }

        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCampo.SelectedIndex>=0)
            {
                string opcion = cbxCampo.SelectedItem.ToString();
                if (opcion == "Número")
                {
                    cbxCriterio.Items.Clear();
                    cbxCriterio.Items.Add("Mayor a");
                    cbxCriterio.Items.Add("Menor a");
                    cbxCriterio.Items.Add("Igual a");
                }
                else
                {
                    cbxCriterio.Items.Clear();
                    cbxCriterio.Items.Add("Empieza con");
                    cbxCriterio.Items.Add("Termina con");
                    cbxCriterio.Items.Add("Contiene");


                }
            }


        }

        private void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            if(txtFiltro.Text!="")txtFiltro.Clear();
            if(txtFiltroAvanzado.Text!="")txtFiltroAvanzado.Clear();
            if(cbxCampo.SelectedIndex >=0) cbxCampo.SelectedIndex = -1;
            if(cbxCriterio.SelectedIndex >=0)cbxCriterio.SelectedIndex = -1;
            cargar();
        }
    }
}
