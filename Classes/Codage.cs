using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lectureImage
{
    /// <summary>
    /// Classe codage contenant l'image codée et les 2 images d'origines décodées
    /// </summary>
    public class Codage
    {
        // Attributs
        private Pixel[,] image_codee;
        private Pixel[,] image_cachante_decodee;        // Image d'origine cachant l'autre image
        private Pixel[,] image_cachee_decodee;          // Image d'origine cachée par l'autre image

        /// <summary>
        /// Constructeur de la classe Codage
        /// </summary>
        /// <param name="image_a_coder1">Image dans laquelle sera cachée une autre image</param>
        /// <param name="image_a_coder2">Image à cacher</param>
        public Codage (MonImage image_a_coder1, MonImage image_a_coder2)
        {
            Pixel[,] codage = null;
            
            // On vérifie que les 2 images sont de mêmes dimensions, sinon on redimensionne l'image à cacher
            if (image_a_coder1.HauteurImage != image_a_coder2.HauteurImage || image_a_coder1.LargeurImage != image_a_coder2.LargeurImage)
            {
                image_a_coder2.Image = Retailler(image_a_coder2, image_a_coder1.HauteurImage, image_a_coder1.LargeurImage);
                image_a_coder2.HauteurImage = image_a_coder2.Image.GetLength(0);
                image_a_coder2.LargeurImage = image_a_coder2.Image.GetLength(1);
                image_a_coder2.TailleFichier = image_a_coder2.HauteurImage * image_a_coder2.LargeurImage * 3 + 54;
            }
            // On peut maintenant coder des images de mêmes dimensions
            if (image_a_coder1.HauteurImage == image_a_coder2.HauteurImage && image_a_coder1.LargeurImage == image_a_coder2.LargeurImage)
            {
                string pixel1B;
                string pixel2B;
                string pixel1V;
                string pixel2V;
                string pixel1R;
                string pixel2R;
                string pixel_binaireB;
                string pixel_binaireV;
                string pixel_binaireR;

                codage = new Pixel[image_a_coder1.HauteurImage, image_a_coder1.LargeurImage];
                for (int ligne = 0; ligne < codage.GetLength(0); ligne++)
                {
                    for (int colonne = 0; colonne < codage.GetLength(1); colonne++)
                    {
                        // On convertit les valeurs des pixels de chaque image de décimal en binaire et ce pour chaque couleur RGB
                        pixel1B = IntToBinary(image_a_coder1.Image[ligne, colonne].Bleu);
                        pixel2B = IntToBinary(image_a_coder2.Image[ligne, colonne].Bleu);
                        pixel1V = IntToBinary(image_a_coder1.Image[ligne, colonne].Vert);
                        pixel2V = IntToBinary(image_a_coder2.Image[ligne, colonne].Vert);
                        pixel1R = IntToBinary(image_a_coder1.Image[ligne, colonne].Rouge);
                        pixel2R = IntToBinary(image_a_coder2.Image[ligne, colonne].Rouge);

                        // On construit la valeur du pixel correspondant de la future image
                        // Puisqu'on cherche ici à cacher l'image 2 dans l'image 1, le pixel de la future image sera composé des 4 premiers chiffres de celui de la 1ere image et des 4 derniers de celui de la 2eme
                        pixel_binaireB = "";
                        pixel_binaireV = "";
                        pixel_binaireR = "";
                        for (int i = 0; i < 8; i++) // grâce à la fonction IntToBinray créée, on est certain d'avoir une chaine de 8 caractères
                        {
                            if (i < 4)
                            {
                                pixel_binaireB += Convert.ToString(pixel1B[i]);
                                pixel_binaireV += Convert.ToString(pixel1V[i]);
                                pixel_binaireR += Convert.ToString(pixel1R[i]);
                            }
                            else
                            {
                                pixel_binaireB += Convert.ToString(pixel2B[i]);
                                pixel_binaireV += Convert.ToString(pixel2V[i]);
                                pixel_binaireR += Convert.ToString(pixel2R[i]);
                            }
                        }

                        // On convertit maintenant la valeur de chaque couleur du pixel en entier exploitable par la matrice de pixels et on rempli la matrice 
                        codage[ligne, colonne] = new Pixel(BinaryToInt(pixel_binaireR), BinaryToInt(pixel_binaireV), BinaryToInt(pixel_binaireB));
                    }
                }
            }
            
            this.image_codee = codage;
            // On on ne remplit pas les matrcies correspondants aux images decodee puiqu'il n'y a pas eu de décodage dans le constructeur
            this.image_cachante_decodee = null;
            this.image_cachee_decodee = null;
        }

        /// <summary>
        /// Propriété de l'attribut image_codee
        /// </summary>
        public Pixel [,] ImageCodee
        {
            get { return image_codee; }
        }

        /// <summary>
        /// Propriété de l'attribut image_cachante_decodee
        /// </summary>
        public Pixel [,] ImageCachante
        {
            get { return image_cachante_decodee; }
        }

        /// <summary>
        /// Propriété de l'attribut image_cachee_decodee
        /// </summary>
        public Pixel[,] ImageCachee
        {
            get { return image_cachee_decodee; }
        }

        // Méthodes

        /// <summary>
        /// Conversion d'un entier en un nombre binaire
        /// </summary>
        /// <param name="entier">Nombre entier à convertir</param>
        /// <returns>Chaine de type string contenant le nombre binaire</returns>
        public string IntToBinary(int entier)
        {
            string binaire = "";
            string rep = "";
            int reste = 0;
            while (entier > 0)
            {
                reste = entier % 2;         // reste de la division euclidienne de l'entier par 2
                entier = entier / 2;        // quotient de la division euclidienne
                binaire += reste;
            }
            // On complète les éléments manquants par des 0 si la taille de la chaîne n'atteint pas 8, ainsi on est certain de toujours avoir une chaine de 8 caractères en retour
            {
                while (binaire.Length < 8)
                    binaire += 0;
            }
            // On inverse la chaine obtenue pour pouvoir l'exploiter convenablement dans la méthode CoderImage
            for (int i = binaire.Length - 1; i >= 0; i--)
            {
                rep += binaire[i];
            }
            return rep;
        }

        /// <summary>
        /// Convertion d'un nombre binaire en nombre entier
        /// </summary>
        /// <param name="binaire">Chaine de type string contenant le nombre binaire à convertir</param>
        /// <returns>Nombre entier converti</returns>
        public int BinaryToInt(string binaire)
        {
            int conversion = 0;
            for (int i = 0; i < binaire.Length; i++)
            {
                if (binaire[i].CompareTo('1') == 0)
                    conversion += (int)Math.Pow(2, binaire.Length - 1 - i);     // conversion += 2^(7-i) 
                // Remarque si binaire[i].CompareTo('1') !=0 cela signifie que binaire[i] == 0, or les puissances de 0 valent toutes 0, il est donc inutile de les ajouter à la variable conversion
            }
            return conversion;
        }

        /// <summary>
        /// Permet de retrouver les 2 images d'origine composant l'image codée
        /// </summary>
        /// <param name="fileName1">Nom du fichier sous lequel on sauvegarde l'image cachante d'origine</param>
        /// <param name="fileName2">Nom du fichier sous lequel on sauvegarde l'image cachée d'origine</param>
        public void Decodage (string fileName1, string fileName2)
        {
            image_cachante_decodee = new Pixel[image_codee.GetLength(0), image_codee.GetLength(1)];
            image_cachee_decodee = new Pixel[image_codee.GetLength(0), image_codee.GetLength(1)];

            // valeurs binaires des composantes d'un pixel de l'image cachante d'origine
            string pixel1B;
            string pixel1V;
            string pixel1R;
            // valeurs binaires des composantes d'un pixel de l'image cachée
            string pixel2B;
            string pixel2V;
            string pixel2R;
            // valeurs binaires des composantes d'un pixel de l'image à décoder
            string pixel_binaireB;
            string pixel_binaireV;
            string pixel_binaireR;

            for (int ligne = 0; ligne < image_codee.GetLength(0); ligne++)
            {
                for (int colonne = 0; colonne < image_codee.GetLength(1); colonne++)
                {
                    // On récupère les valeurs binaires des composantes RGB de chaque pixel de l'image à décoder
                    pixel_binaireB = IntToBinary(image_codee[ligne, colonne].Bleu);
                    pixel_binaireV = IntToBinary(image_codee[ligne, colonne].Vert);
                    pixel_binaireR = IntToBinary(image_codee[ligne, colonne].Rouge);

                    // On reconstitue les composantes du pixel de l'image d'origine et du pixel l'image cachée
                    pixel1B = ""; pixel2B = ""; pixel1V = ""; pixel2V = ""; pixel1R = ""; pixel2R = "";
                    for (int i = 0; i < 4; i++)
                    {
                        // Chaque composante RGB du pixel de l'image d'origine est composée des 4 premiers bits de la chaine de caractère obtenue juste au-dessus ("pixel_binaire")
                        pixel1B += pixel_binaireB[i];
                        pixel1V += pixel_binaireV[i];
                        pixel1R += pixel_binaireR[i];

                        // Chaque composante RGB du pixel de l'image cachée est composée des 4 derniers bits de la chaine de caractère obtenue juste au dessus
                        pixel2B += pixel_binaireB[4 + i];
                        pixel2V += pixel_binaireV[4 + i];
                        pixel2R += pixel_binaireR[4 + i];
                    }

                    // On complète les informations manquantes par des 0 (cela altérera la qualité des images d'origines mais nous permettra tout de même de nous en approcher)
                    for (int i = 0; i < 4; i++)
                    {
                        pixel1B += 0;
                        pixel1V += 0;
                        pixel1R += 0;
                        pixel2B += 0;
                        pixel2V += 0;
                        pixel2R += 0;
                    }

                    // On convertit les valeurs binaires des 3 composantes de chaque pixel en valeurs décimales pour remplir les matrices de pixels associées
                    image_cachante_decodee[ligne, colonne] = new Pixel(BinaryToInt(pixel1R), BinaryToInt(pixel1V), BinaryToInt(pixel1B));
                    image_cachee_decodee[ligne, colonne] = new Pixel(BinaryToInt(pixel2R), BinaryToInt(pixel2V), BinaryToInt(pixel2B));
                }
            }

            // On sauvegarde l'image cachante d'origne en fichier bmp
            MonImage image = new MonImage(image_codee.GetLength(0), image_codee.GetLength(1), image_cachante_decodee);
            image.From_Image_To_File(fileName1);

            // On sauvegarde l'image cachée d'origine en fichier bmp
            image.Image = image_cachee_decodee;
            image.From_Image_To_File(fileName2);
        }

        /// <summary>
        /// Crée une matrice de pixels contenant une autre matrice de pixels et dont la hauteur et la largeur sont entrées en paramètre
        /// </summary>
        /// <param name="image">Matrice de pixels que l'on souhaite recopier dans la nouvelle image</param>
        /// <param name="hauteur">Hauteur de la nouvelle matrice de pixels</param>
        /// <param name="largeur">Largeur de la nouvelle matrice de pixels</param>
        /// <returns></returns>
        public Pixel [,] Retailler (MonImage image, int hauteur, int largeur)
        {
            Pixel[,] newImage = new Pixel[hauteur, largeur];
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                for (int colonne =0; colonne < largeur; colonne++)
                {
                    if (ligne < image.HauteurImage && colonne < image.LargeurImage)
                    {
                        newImage[ligne, colonne] = image.Image[ligne, colonne];
                    }
                    else newImage[ligne, colonne] = new Pixel(255, 255, 255);       // On complète les éléments manquants par des pixels blancs
                    
                }
            }
            return newImage;
        }
    }
}
