using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lectureImage
{
    /// <summary>
    /// Classe Histogramme rassemblant les 4 histogrammes d'une image
    /// </summary>
    public class Histogramme
    {
        // Attributs
        private Pixel[,] histoRouge;
        private Pixel[,] histoVert;
        private Pixel[,] histoBleu;
        private Pixel[,] histoGeneral;      // histogramme géneral des 3 composantes

        // Attributs utiles pour la normaliation d'histogrammes d'une image en niveaux de gris
        private int maxNG;      // représente la valeur maximale ( en terme d'intensité) prise par les pixels dans une image en niveaux de gris
        private int minNG;      // représente la valeur minimale ( en terme d'intensité) prise par les pixels dans une image en niveaux de gris

        /// <summary>
        /// Constructeur de la classe Histogramme
        /// </summary>
        /// <param name="image">Instance de la classe MonImage dont on veut obtenir les histogrammes</param>
        public Histogramme(MonImage image)
        {
            // Les dimensions des histogrammes sont fixées à 100x256 pixels
            histoBleu = new Pixel[100, 256];
            histoRouge = new Pixel[100, 256];
            histoVert = new Pixel[100, 256];
            histoGeneral = new Pixel[100, 256];
            // On crée 4 tableaux représentant la répartition des intensités des composantes de chaque pixel
            int[] repartition_pixelsB = new int[256];
            int[] repartition_pixelsV = new int[256];
            int[] repartition_pixelsR = new int[256];
            int[] repartition_totale = new int[256];

            for (int ligne = 0; ligne < image.HauteurImage; ligne++)
            {
                for (int colonne = 0; colonne < image.LargeurImage; colonne++)
                {
                    repartition_pixelsB[image.Image[ligne, colonne].Bleu] += 1;
                    repartition_pixelsV[image.Image[ligne, colonne].Vert] += 1;
                    repartition_pixelsR[image.Image[ligne, colonne].Rouge] += 1;
                }
            }

            int maxB = 0;
            int maxV = 0;
            int maxR = 0;
            int max = 0;
            
            // On détermine le nombre maximum d'occurences que peut prendre un pixel cela nous permetra de déterminer combien de pixels de l'histogramme sont à colorer selon le rapport (100/max)
            for (int i = 0; i < 256; i++)
            {
                if (repartition_pixelsB[i] > maxB)
                {
                    maxB = repartition_pixelsB[i];
                }
                if (repartition_pixelsV[i] > maxV)
                {
                    maxV = repartition_pixelsV[i];
                }
                if (repartition_pixelsR[i] > maxR)
                {
                    maxR = repartition_pixelsR[i];
                }

                // On remplit par la même occasion le tableau contenant le nombre total d'occurences pour chacune des 256 valeurs possibles de pixel
                repartition_totale[i] = repartition_pixelsB[i] + repartition_pixelsR[i] + repartition_pixelsV[i];
                if (repartition_totale[i] > max)
                {
                    max = repartition_totale[i];
                }
            }

            // Ici on dététermine les valeurs maximales et minimales prises par les pixels de l'image en niveau de gris
            int indice = -1;
            do
            {
                indice++;
                this.minNG = indice;
            } while (repartition_totale[indice] == 0);

            indice = 256;
            do
            {
                indice--;
                this.maxNG = indice;
            } while (repartition_totale[indice] == 0);

            // On initialise chaque case de la matrice représentant l'histogramme à un pixel de couleur noire
            for (int i = 0; i <100; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    histoBleu[i, j] = new Pixel(0, 0, 0);
                    histoRouge[i, j] = new Pixel(0, 0, 0);
                    histoVert[i, j] = new Pixel(0, 0, 0);
                    histoGeneral[i, j] = new Pixel(0, 0, 0);
                }
            }
            // On rempli chaque histogramme suivant les valeurs contenues dans les tableaux
            for (int colonne = 0; colonne < 256; colonne++)
            {
                for (int ligne = histoBleu.GetLength(0) - 1; ligne >= histoBleu.GetLength(0) - (repartition_pixelsB[colonne] * 100 / maxB); ligne--)
                {
                    histoBleu[ligne, colonne].Bleu = 255;
                }
                for (int ligne = histoVert.GetLength(0) - 1; ligne >= histoVert.GetLength(0) - (repartition_pixelsV[colonne] * 100 / maxV); ligne--)
                {
                    histoVert[ligne, colonne].Vert = 255;
                }
                for (int ligne = histoRouge.GetLength(0) - 1; ligne >= histoRouge.GetLength(0) - (repartition_pixelsR[colonne] * 100 / maxR); ligne--)
                {
                    histoRouge[ligne, colonne].Rouge = 255;
                }
                for (int ligne = histoGeneral.GetLength(0) - 1; ligne >= histoGeneral.GetLength(0) - (repartition_totale[colonne] * 100 / max); ligne--)
                {
                    histoGeneral[ligne, colonne].Rouge = 255;
                    histoGeneral[ligne, colonne].Vert = 255;
                    histoGeneral[ligne, colonne].Bleu = 255;
                }
            }
        }

        /// <summary>
        /// Propriété de l'attribut histoRouge
        /// </summary>
        public Pixel [,] HistoRouge
        {
            get { return histoRouge; }
        }

        /// <summary>
        /// Propriété de l'attribut histoRVert
        /// </summary>
        public Pixel[,] HistoVert
        {
            get { return histoVert; }
        }

        /// <summary>
        /// Propriété de l'attribut histoBleu
        /// </summary>
        public Pixel[,] HistoBleu
        {
            get { return histoBleu; }
        }

        /// <summary>
        /// Propriété de l'attribut histoGeneral des 3 composantes
        /// </summary>
        public Pixel[,] HistoGeneral
        {
            get { return histoGeneral; }
        }

        /// <summary>
        /// Propriété de l'attribut MaxNG
        /// </summary>
        public int MaxNG
        {
            get { return maxNG; }
        }

        /// <summary>
        /// Propriété de l'attribut MinNG
        /// </summary>
        public int MinNG
        {
            get { return minNG; }
        }
    }
}
