using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using System.Configuration;

namespace Pokemons
{
    public partial class FrmAltaPokemon : Form
    {

        private Pokemon pokemon = null;
        private OpenFileDialog archivo = null;

        public FrmAltaPokemon()
        {
            InitializeComponent();
        }

        public FrmAltaPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon;
            Text = "Modificar Pokemon";
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
           Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            PokemonNegocio negocio = new PokemonNegocio();
            try
            {
                if (pokemon == null)
                    pokemon = new Pokemon();

                pokemon.Numero = int.Parse(txtNumero.Text);
                pokemon.Nombre = txtNombre.Text;
                pokemon.Descripcion = txtDescripcion.Text;
                pokemon.UrlImagen = txtUrlImagen.Text;
                pokemon.Tipo = (Elemento)cboTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cboDebilidad.SelectedItem;


                if (pokemon.Id == 0)
                {
                    negocio.Agregar(pokemon);
                    MessageBox.Show("Agregado exitosamente");
                }
                else
                {
                    negocio.Modificar(pokemon);
                    MessageBox.Show("Modificado exitosamente");
                }
                if (archivo!=null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["PokemonIMG"] + archivo.SafeFileName);
                }


                Close();



            }
            catch (Exception ex)
            {

                MessageBox.Show("Ingrese todos los campos necesarios");
            }
        }

        private void lblNombre_Click(object sender, EventArgs e)
        {

        }

        private void FrmAltaPokemon_Load(object sender, EventArgs e)
        {
            ElementoNegocio elementoNegocio = new ElementoNegocio();
            try
            {
                cboTipo.DataSource = elementoNegocio.listar();
                cboTipo.ValueMember = "Id";
                cboTipo.DisplayMember = "Descripcion";
                cboDebilidad.DataSource = elementoNegocio.listar();
                cboDebilidad.ValueMember = "Id";
                cboDebilidad.DisplayMember = "Descripcion";
                cboDebilidad.SelectedIndex = -1;
                cboTipo.SelectedIndex = -1;

                if (pokemon!= null)
                {
                    txtNumero.Text = pokemon.Numero.ToString();
                    txtNombre.Text = pokemon.Nombre;
                    txtDescripcion.Text = pokemon.Descripcion;
                    txtUrlImagen.Text = pokemon.UrlImagen;
                    cargarImagen(pokemon.UrlImagen);
                    cboTipo.SelectedValue = pokemon.Tipo.Id;
                    cboDebilidad.SelectedValue = pokemon.Debilidad.Id;
                    
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception ex)
            {

                 pbxPokemon.Load("https://plantillasdememes.com/img/plantillas/imagen-no-disponible01601774755.jpg");
            }

        }

        private void txtUrlImagen_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|png|*.png";
            if (archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
            }
            



        }
    }
}
