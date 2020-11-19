using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace lectureImage
{
    // Julie ANDRE TDG

    /// <summary>
    /// Classe Program 
    /// </summary>
    public class Program
    {
       
        /// <summary>
        /// Demande à l'utilisateur de saisir un entier tant que sa valeur n'est pas valide, c'est à dire entière, strictement positive et plus petite que la valeur max
        /// </summary>
        /// <param name="valmax"> Valeur maximale que peut prendre l'entier saisi sur la console </param>
        /// <returns></returns>
        static public int SaisieNombre(int valmax)
        {
            int result;
            bool verified;
            
            verified = int.TryParse(Console.ReadLine(), out result); // se met dans result si réussi 
            while (!verified || result <= 0 || result > valmax )
            {
                Console.WriteLine("Erreur de saisie, reessayez");
                verified = int.TryParse(Console.ReadLine(), out result);
            }
            return result;
        }

        /// <summary>
        /// Demande à l'utilisateur de saisir un réel tant que sa valeur n'est pas valide, c'est à dire strictement positive et plus petite que la valeur max
        /// </summary>
        /// <param name="valeurmax">Valeur maximale que peut prendre le réel saisi sur la console</param>
        /// <returns></returns>
        static public double SaisieFacteur(int valeurmax)
        {
            double result;

            bool verified;
            verified = double.TryParse(Console.ReadLine(), out result);
            while (!verified || result <= 0 || result > valeurmax)
            {
                Console.WriteLine("Erreur de saisie, reessayez");
                verified = double.TryParse(Console.ReadLine(), out result);
            }
            return result;
        }

        static void Main(string[] args)
        {

            ConsoleKeyInfo cki;

            // On permet à l'utilisateur de choisir l'image à laquelle il pourra appliquer des traitements 
            Console.WriteLine(" Avec quelle image souhaitez-vous travailler ? :\n"
                                 + " 1 : coco \n"
                                 + " 2 : lac en montagne \n"
                                 + " 3 : lena \n"
                                 + "\n Sélectionnez l'option désirée en entrant son numéro ");
            int choix = SaisieNombre(3);
            string NomFichier = "";
            if (choix == 1) NomFichier = "coco.bmp";
            else if (choix == 2) NomFichier = "lac_en_montagne.bmp";
            else NomFichier = "lena.bmp";

            do
            {
                Console.Clear();
                Console.WriteLine("\n                    Menu \n"
                                 + " _____________________________________________ \n\n"
                                 + "  1 : Niveaux de Gris \n"
                                 + "  2 : Noir et Blanc \n"
                                 + "  3 : Rotations \n"
                                 + "  4 : Effet miroir \n"
                                 + "  5 : Agrandissement/Rétrécissment \n"
                                 + "  6 : Filtres \n"
                                 + "  7 : Fractales \n"
                                 + "  8 : Histogrammes \n"
                                 + "  9 : Codage /Décodage d'images \n"
                                 + " 10 : Innovation : Normalisation d'histogramme \n"
                                 + " _____________________________________________ \n"
                                 + "\n Sélectionnez l'option désirée en entrant son numéro ");
                int option = SaisieNombre(10);
                int optionB;

                // On recré une instance d'image quand on en a besoin dans les différents cas pour ne pas garder les modifications précédentes qui risquent de fosser le résultat
                switch (option)
                {
                    #region
                    case 1:
                        MonImage ImageTest = new MonImage(NomFichier);
                        ImageTest.NiveauxGris();
                        ImageTest.From_Image_To_File("Sortie.bmp");
                        break;
                    case 2:
                        ImageTest = new MonImage(NomFichier);
                        ImageTest.NoirBlanc();
                        ImageTest.From_Image_To_File("Sortie.bmp");
                        break;
                    case 3:
                        ImageTest = new MonImage(NomFichier);
                        Console.WriteLine("\n 3 rotations possibles :\n"
                                 + " 1 : Rotation 90° \n"
                                 + " 2 : Rotation 180° \n"
                                 + " 3 : Rotation 270° \n"
                                 + "\n Sélectionnez l'option désirée en entrant son numéro ");
                        optionB = SaisieNombre(3);
                        switch (optionB)
                        {
                            #region
                            case 1:
                                ImageTest.Rotation_90();
                                break;
                            case 2:
                                ImageTest.Rotation_180();
                                break;
                            case 3:
                                ImageTest.Rotation_270();
                                break;

                            #endregion
                        }
                        ImageTest.From_Image_To_File("Sortie.bmp");
                        break;
                    case 4:
                        ImageTest = new MonImage(NomFichier);
                        ImageTest.Miroir();
                        ImageTest.From_Image_To_File("Sortie.bmp");
                        break;
                    case 5:
                        ImageTest = new MonImage(NomFichier);
                        Console.WriteLine("\n2 choix possibles :\n"
                                 + " 1 : Agrandissement \n"
                                 + " 2 : Rétrécissement \n"
                                 + "\n Sélectionnez l'option désirée en entrant son numéro ");
                        optionB = SaisieNombre(2);
                        double facteur;
                        switch (optionB)
                        {
                            #region
                            case 1:
                                Console.WriteLine(" Saisissez un facteur d'agrandissement ex : 2 ; 10 ; 17; 2,5 (des entiers sont recommandés pour un meilleur rendu)");
                                facteur = SaisieFacteur(50);      // On met 50 en paramètre pour ne pas avoir une image trop grande
                                ImageTest.Agrandir(facteur);
                                break;
                            case 2:
                                do
                                {
                                    Console.WriteLine(" Saisissez un facteur de rétrécissement qui est diviseur de la largeur : " + ImageTest.LargeurImage + " et de la hauteur de l'image : " + ImageTest.HauteurImage);
                                    facteur = SaisieNombre(Math.Min(ImageTest.HauteurImage, ImageTest.LargeurImage));
                                } while (ImageTest.HauteurImage % facteur != 0 || ImageTest.LargeurImage % facteur != 0);
                                
                                ImageTest.Retrecir(facteur);
                                break;
                                #endregion
                        }
                        Console.WriteLine("\n Infos image obtenue après modifications : \n" + ImageTest.toString());
                        ImageTest.From_Image_To_File("Sortie.bmp");
                        break;
                    case 6:
                        ImageTest = new MonImage(NomFichier);
                        Console.WriteLine("\n4 filtres possibles :\n"
                                 + " 1 : Détection des bords \n"
                                 + " 2 : Renforcement des bords \n"
                                 + " 3 : Flou \n"
                                 + " 4 : Repoussage \n"
                                 + "\n Sélectionnez l'option désirée en entrant son numéro ");
                        optionB = SaisieNombre(4);
                        int[,] mat_convolution;
                        switch (optionB)
                        {
                            #region
                            case 1:
                                mat_convolution = new int[,] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
                                Pixel[,] detectionDesBords = ImageTest.Filtre(mat_convolution, false);
                                ImageTest.Image = detectionDesBords;
                                break;
                            case 2:
                                mat_convolution = new int[,] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
                                Pixel[,] renforcementDesBords = ImageTest.Filtre(mat_convolution, false);
                                ImageTest.Image = renforcementDesBords;
                                break;
                            case 3:
                                mat_convolution = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
                                Pixel[,] flou = ImageTest.Filtre(mat_convolution, true);
                                ImageTest.Image = flou;
                                break;
                            case 4:
                                mat_convolution = new int[,] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
                                Pixel[,] repoussage = ImageTest.Filtre(mat_convolution, false);
                                ImageTest.Image = repoussage;
                                break;
                                #endregion
                        }
                        ImageTest.From_Image_To_File("Sortie.bmp");
                        break;
                    case 7:
                        Console.WriteLine("\n 2 fractales possibles :\n"
                                 + " 1 : Fractale de Mandelbrot \n"
                                 + " 2 : Fractale de Julia \n"
                                 + "\n Sélectionnez l'option désirée en entrant son numéro ");
                        optionB = SaisieNombre(2);
                        switch (optionB)
                        {
                            #region
                            case 1:
                                Console.WriteLine("\n Cela peut prendre quelques secondes ... \n");
                                Fractale Mandelbrot = new Fractale("Mandelbrot", 500); 
                                MonImage fractale = new MonImage(Mandelbrot.Image.GetLength(0), Mandelbrot.Image.GetLength(1), Mandelbrot.Image);
                                Console.WriteLine("Infos image obtenue : \n" + fractale.toString());
                                fractale.From_Image_To_File("fractaleM.bmp");
                                break;
                            case 2:
                                Console.WriteLine("\n Cela peut prendre quelques secondes ... \n");
                                Fractale Julia = new Fractale("Julia", 500); 
                                MonImage fractaleJ = new MonImage(Julia.Image.GetLength(0), Julia.Image.GetLength(1), Julia.Image);
                                Console.WriteLine("Infos image obtenue : \n" + fractaleJ.toString());
                                fractaleJ.From_Image_To_File("julia.bmp"); 
                                break;
                            #endregion
                        }
                        break;
                       
                    case 8:
                        ImageTest = new MonImage(NomFichier);
                        Histogramme histogrammes = new Histogramme(ImageTest);
                        MonImage histoR = new MonImage(100, 256, histogrammes.HistoRouge); // 100 et 256 seront toujours les dimensions des histogrammes créés dans le constructeur de la classe Histogramme
                        MonImage histoV = new MonImage(100, 256, histogrammes.HistoVert);
                        MonImage histoB = new MonImage(100, 256, histogrammes.HistoBleu);       
                        MonImage histoG = new MonImage(100, 256, histogrammes.HistoGeneral);
                        histoR.From_Image_To_File("histoR.bmp");
                        histoV.From_Image_To_File("histoV.bmp");
                        histoB.From_Image_To_File("histoB.bmp");
                        histoG.From_Image_To_File("histoG.bmp");
                        Console.WriteLine("\n Les 4 histogrammes correspondant à l'image ont été créés\n");
                        break;
                    case 9:
                        ImageTest = new MonImage(NomFichier);
                        MonImage ImageAcacher = new MonImage("image_a_cacher2.bmp");
                        Codage codage = new Codage(ImageTest, ImageAcacher);
                        ImageTest.Image = codage.ImageCodee;
                        ImageTest.From_Image_To_File("ImageCachee.bmp");

                        Console.WriteLine("\nParmi les 3 images qui s'affichent : \n L'image cachante est " + NomFichier +",\n L'image à cacher est image_a_cacher2.bmp,\n L'image codée obtenue est ImageCachee.bmp");
                        Console.WriteLine("\n Souhaitez vous décoder l'image obtenue pour retrouver les 2 images d'origine ? \n"
                                 + " 1 : Oui \n"
                                 + " 2 : Non \n"
                                 + "\n Sélectionnez l'option désirée en entrant son numéro ");
                        optionB = SaisieNombre(2);
                        if (optionB == 1)
                        {
                            codage.Decodage("image_origine.bmp", "image_cachee.bmp");
                            Console.WriteLine("\nParmi les 2 images qui s'affichent : \n L'image d'origine est image_origine.bmp,\n L'image qui était cachée est image_cachee.bmp");
                        }
                        break;
                    case 10:
                        // On va travailler ici avec une image déjà en niveaux de gris (lena)
                        MonImage lena = new MonImage("lena.bmp");
                        Histogramme histo = new Histogramme(lena);

                        // On normalise l'image à partir de son histogramme
                        lena.Normalisation(histo);

                        // On récupère le nouvel histogramme de l'image normalisée
                        Histogramme histo_normalise = new Histogramme(lena);

                        // On convertit cet histogramme en instance de la classe MonImage pour pouvoir le convertir en fichie bmp
                        MonImage imagehisto = new MonImage(histo.HistoGeneral.GetLength(0), histo.HistoGeneral.GetLength(1), histo_normalise.HistoGeneral);
                        lena.From_Image_To_File("lenaS.bmp");
                        imagehisto.From_Image_To_File("lenaH.bmp");
                        Console.WriteLine("\n 3 images se sont affichées :\n L'image d'origine lena.bmp,\n L'image obtenue après normalisation lenaS.bmp,\n L'histogramme obtenu après normalisation lenaH.bmp");
                        break;
                    default:
                        break;

                    #endregion
                }
                Console.WriteLine("\n Tapez sur la touche Escape pour sortir ou sur la touche Enter pour continuer");
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);


            Console.ReadKey();
        }
    }
}
