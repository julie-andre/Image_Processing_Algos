using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lectureImage
{
    /// <summary>
    /// Classe Pixel modélisant un pixel caractérisé par ses 3 composantes RVB
    /// </summary>
    public class Pixel
    {
        //Attributs
        private int rouge;
        private int vert;
        private int bleu;

        
        /// <summary>
        /// Constructeur de la classe Pixel
        /// </summary>
        /// <param name="rouge">Intensité de la composante rouge du pixel</param>
        /// <param name="vert">Intensité de la composante verte du pixel</param>
        /// <param name="bleu">Intensité de la composante bleue du pixel</param>
        public Pixel(int rouge, int vert, int bleu)
        {
            if (0 <=rouge && rouge <= 255)
            {
                this.rouge = rouge;
            }
            if (0 <= vert && vert <= 255)
            {
                this.vert = vert;
            }
            if (0 <= bleu && bleu <= 255)
            {
                this.bleu = bleu;
            }   
        }

        /// <summary>
        /// Propriété de l'attribut rouge
        /// </summary>
        public int Rouge
        {
            get { return rouge; }
            set { rouge = value; }
        }

        /// <summary>
        /// Propriété de l'attribut vert
        /// </summary>
        public int Vert
        {
            get { return vert; }
            set { vert = value; }
        }

        /// <summary>
        /// Propriété de l'attribut bleu
        /// </summary>
        public int Bleu
        {
            get { return bleu; }
            set { bleu = value; }
        }

        // Méthodes

        /// <summary>
        /// Crée une chaine contenant les différentes valeurs des composantes du pixel
        /// </summary>
        /// <returns> Chaine de type string contenant ces informations </returns>
        public string toString()
        {
            string rep = rouge + "\t" + vert + "\t" + bleu + "\t";
            return rep;
        }

        /// <summary>
        /// Change la valeur du pixel pour qu'il corresponde au pixel d'une image en niveaux de gris
        /// </summary>
        public void NiveauxDeGris()
        {
            int gris = (rouge + vert + bleu) / 3;
            rouge = gris;
            vert = gris;
            bleu = gris;
        }

        /// <summary>
        /// Change la valeur du pixel pour qu'il corresponde au pixel d'une image en noir et blanc
        /// </summary>
        public void NoirEtBlanc()
        {
            int couleur = (rouge + vert + bleu) / 3;
            if (couleur < 128) couleur = 0;
            else couleur = 255;
            rouge = couleur;
            vert = couleur;
            bleu = couleur;

        }

    }
}
