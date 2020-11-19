using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lectureImage
{
    /// <summary>
    /// Classe Fractale permettant de créer 2 types différents de fractales
    /// </summary>
    public class Fractale
    {
        // Attribut
        private Pixel[,] image;

        /// <summary>
        /// Constructeur de la classe Fractale
        /// </summary>
        /// <param name="type">Type de fractale à créer (Mandelbrot ou Julia)</param>
        /// <param name="zoom">Détermine la taille future de la fractale</param>
        public Fractale (string type, double zoom)
        {
            if(type.CompareTo("Mandelbrot") == 0)
            {
                Mandelbrot(zoom);
            }
            else
            {
                Julia(zoom);
            }
            
        }

        /// <summary>
        /// Propriété de l'attribut image
        /// </summary>
        public Pixel [,] Image
        {
            get { return image; }
        }

        // Méthodes

        /// <summary>
        /// Modifie l'attribut image (matrice de pixels) pour qu'il corresponde à une fractale de Mandelbrot
        /// </summary>
        /// <param name="zoom"> Détermine la taille de la fractale </param>
        public void Mandelbrot(double zoom)
        {
            // On définit les limites du repère des fractales de Mandelbrot
            double xmin = -2.1;
            double xmax = 0.6;
            double ymin = -1.2;
            double ymax = 1.2;

            // On détermine les dimensions de la matrice de pixels représentant l'image, 1 sur le repère est représenté par zoom pixels sur l'image
            int hauteur_image = (int)((xmax - xmin) * zoom);
            int largeur_image = (int)((ymax - ymin) * zoom);
            this.image = new Pixel[hauteur_image, largeur_image];

            // On définit les coordonnées dans le plan complexe des nombres Z et C intervenant dans la relation de récurrence définissant l'appartenance du point Z à l'ensemble de Mandelbrot
            double C_r;
            double C_i;
            double z_r;
            double z_i;
            int i;
            double module;
            double tmp;

            //Permet d'arrêter le calcul lorsque le module du point n'atteint jamais 2 du fait que ce point n'appartient pas à l'ensemble de Mandelbrot
            int iteration_max = 100;

            for (int x = 0; x < hauteur_image; x++)
            {
                for (int y = 0; y < largeur_image; y++)
                {
                    C_r = x / zoom + xmin;
                    C_i = y / zoom + ymin;
                    z_r = 0;
                    z_i = 0;
                    i = 0;
                    module = 0;

                    do
                    {
                        tmp = z_r;
                        z_r = z_r * z_r - z_i * z_i + C_r;
                        z_i = 2 * z_i * tmp + C_i;
                        module = Math.Pow(z_r * z_r * +z_i * z_i, 0.5);
                        i++;
                    } while (module < 2 && i < iteration_max);      // si le module dépasse 2 ou que l'on répète trop de fois le calcule, on arrête 

                    if (module < 2)       // si le point appartient à l'ensemble de Mandelbrot
                    {
                        this.image[x, y] = new Pixel(0, 0, 0);
                    }
                    else if (i < iteration_max / 5)
                    {
                        this.image[x, y] = new Pixel(0,0, (int)i * 255 / iteration_max);
                    }
                    else if (i < 2*iteration_max/5)
                    {
                        this.image[x, y] = new Pixel(0, (int)i * 255 / iteration_max, (int)i * 255 / iteration_max);
                    }
                    else if (i <= iteration_max)
                    {
                        this.image[x, y] = new Pixel(0, (int)i * 255 / iteration_max, 0);
                    }
                }
            }

        }

        /// <summary>
        /// Modifie l'attribut image (matrice de pixels) pour qu'il corresponde à une fractale de Julia
        /// </summary>
        /// <param name="zoom"> Détermine la taille de la fractale </param>
        public void Julia (double zoom)
        {
            // On définit les limites d'un repère pouvant contenir la fractale de Julia
            double xmin = -2;
            double xmax = 2;
            double ymin = -1.2;
            double ymax = 1.2;

            // On détermine les dimensions de la matrice de pixels représentant l'image, 1 sur le repère est représenté par zoom pixels sur l'image
            int hauteur_image = (int)((xmax - xmin) * zoom);
            int largeur_image = (int)((ymax - ymin) * zoom);
            this.image = new Pixel[hauteur_image, largeur_image];

            // On définit les coordonnées dans le plan complexe des nombres Z et C intervenant dans la relation de récurrence définissant l'appartenance du point Z à l'ensemble de Julia
            double C_r = -0.8;
            double C_i = 0.156;
            double z_r;
            double z_i;
            int i;
            double module;
            double tmp;

            //Permet d'arrêter le calcul lorsque le module du point n'atteint jamais 2 du fait que ce point n'appartient pas à l'ensemble de Julia
            int iteration_max = 150;

            for (int x = 0; x < hauteur_image; x++)
            {
                for (int y = 0; y < largeur_image; y++)
                {
                    z_r = x / zoom + xmin;
                    z_i = y / zoom + ymin;
                    i = 0;
                    module = 0;

                    do
                    {
                        tmp = z_r;
                        z_r = z_r * z_r - z_i * z_i + C_r;
                        z_i = 2 * z_i * tmp + C_i;
                        module = Math.Pow(z_r * z_r * +z_i * z_i, 0.5);
                        i++;
                    } while (module < 2 && i < iteration_max);      // si le module dépasse 2 ou que l'on répète trop de fois le calcule, on arrête 

                    if (module < 2)       // si le point appartient à l'ensemble de Mandelbrot
                    {
                        this.image[x, y] = new Pixel(255, 125, 0);
                    }
                    else if (iteration_max < iteration_max / 3)
                    {
                        this.image[x, y] = new Pixel((int)i * 255 / iteration_max, 0, 0);
                    }
                    else if (i < 2 * iteration_max / 3)
                    {
                        this.image[x, y] = new Pixel((int)i * 255 / iteration_max, (int)i * 255 / iteration_max, 0);
                    }
                    else if (i <= iteration_max)
                    {
                        this.image[x, y] = new Pixel(0, (int)i * 255 / iteration_max, (int)i * 255 / iteration_max);
                    }
                }
            }
        }

    }
}
