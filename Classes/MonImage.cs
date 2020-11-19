using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace lectureImage
{
    /// <summary>
    /// Classe MonImage permettant de manipuler l'image ainsi que l'enregistrer
    /// </summary>
    public class MonImage
    {
        // Attributs
        private string typeImage;
        private int tailleFichier;
        private int tailleOffset;
        private int hauteurImage;
        private int largeurImage;
        private int nbreBitsParCouleur;
        private Pixel[,] image;


        /// <summary>
        /// Constructeur créant une instance de la classe MonImage à partir d'un fichier dont le nom est entré en paramètre
        /// </summary>
        /// <param name="myfile">Nom du fichier</param>
        public MonImage(string myfile)
        {
            try
            {
                byte[] fichier = null;
                if (myfile[myfile.Length - 1].CompareTo('v') == 0)
                {
                    fichier = ReadFilecsv(myfile);
                }
                else if (myfile[myfile.Length - 1].CompareTo('p') == 0)
                {
                    fichier = File.ReadAllBytes(myfile);
                }
                Process.Start(myfile);

                char lettre1 = Convert.ToChar(fichier[0]);
                char lettre2 = Convert.ToChar(fichier[1]);
                typeImage = Convert.ToString(lettre1) + Convert.ToString(lettre2);

                byte[] bytes = new byte[4];  // sur 4 octets
                for (int i = 2; i < 6; i++)
                {
                    bytes[i - 2] = fichier[i];
                }
                tailleFichier = Convertir_Endian_To_Int(bytes);

                for (int i = 10; i < 14; i++)
                {
                    bytes[i - 10] = fichier[i];
                }
                tailleOffset = Convertir_Endian_To_Int(bytes);

                for (int i = 18; i < 22; i++)
                {
                    //largeurImage += Convert.ToInt32(fichier[i]);
                    bytes[i - 18] = fichier[i];
                }
                largeurImage = Convertir_Endian_To_Int(bytes);

                for (int i = 22; i < 26; i++)
                {
                    //hauteurImage += Convert.ToInt32(fichier[i]);
                    bytes[i - 22] = fichier[i];
                }
                hauteurImage = Convertir_Endian_To_Int(bytes);

                bytes = new byte[2];
                // Revoir cette étape, pb de conversion si jamais on est sur plus de 1 octet
                for (int i = 28; i < 30; i++)
                {
                    nbreBitsParCouleur += Convert.ToInt32(fichier[i]);
                }


                image = new Pixel[hauteurImage, largeurImage];
                int k = 54;
                for (int i = image.GetLength(0) - 1; i >= 0 && k < fichier.Length; i--)
                {
                    for (int j = 0; j < image.GetLength(1) && k < fichier.Length; j++)
                    {
                        image[i, j] = new Pixel(fichier[k + 2], fichier[k + 1], fichier[k]);
                        k += 3;
                    }
                }

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Le fichier n'existe pas " + e.Message);
            }
            finally
            {
                // Console.WriteLine(" fin de la procédure de lecture d'image");
            }
        }

        /// <summary>
        /// Constructeur créant une instance sans partir d'un fichier, à partir de rien
        /// </summary>
        /// <param name="hauteurImage">Hauteur de l'image désirée</param>
        /// <param name="largeurImage">Largeur de l'image désirée</param>
        /// <param name="image">Matrice de pixels représentant l'image</param>
        public MonImage(int hauteurImage, int largeurImage, Pixel[,] image) // permet de créer des images 24 bits de type bmp
        {
            this.hauteurImage = hauteurImage;
            this.largeurImage = largeurImage;
            tailleOffset = 54;
            tailleFichier = hauteurImage * largeurImage * 3 + 54;
            nbreBitsParCouleur = 24;
            typeImage = "BM";

            this.image = image;
        }

        // Propriétés

        /// <summary>
        /// Propriété de l'attribut typeImage
        /// </summary>
        public string TypeImage
        {
            get { return typeImage; }
        }

        /// <summary>
        /// Propriété de l'attribut tailleFichier
        /// </summary>
        public int TailleFichier
        {
            get { return tailleFichier; }
            set { tailleFichier = value; }      // La taille est modifiée lors de l'appel de la fonction Retailler() dans le constructeur de la classe Codage
        }

        /// <summary>
        /// Propriété de l'attribut TailleOffset
        /// </summary>
        public int TailleOffset
        {
            get { return tailleOffset; }
        }

        /// <summary>
        /// Propriété de l'attribut HauteurImage
        /// </summary>
        public int HauteurImage
        {
            get { return hauteurImage; }
            set { hauteurImage = value; }       // La hauteur est modifiée lors de l'appel de la fonction Retailler() dans le constructeur de la classe Codage
        }

        /// <summary>
        /// Propriété de l'attribut LargeurImage
        /// </summary>
        public int LargeurImage
        {
            get { return largeurImage; }
            set { largeurImage = value; }       // La hauteur est modifiée lors de l'appel de la fonction Retailler() dans le constructeur de la classe Codage
        }

        /// <summary>
        /// Propriété de l'attribut nbreBitsParCouleur
        /// </summary>
        public int NbreBitsParCouleur
        {
            get { return nbreBitsParCouleur; }
        }

        /// <summary>
        /// Propriété de l'attribut image
        /// </summary>
        public Pixel [,] Image
        {
            get { return image; }
            set { image = value; }          // L'attribut doit pouvoir être modifié lors du codage de l'image et de l'application des filtres dans le Main
        }

        // Méthodes

        /// <summary>
        /// Complément du constructeur qui permet de lire un fichier de type csv 
        /// </summary>
        /// <param name="filename"> Nom du fichier </param> 
        /// <returns> Un tableau de bytes similaire à celui fourni par la lecture d'un fichier bmp </returns> 
        public byte[] ReadFilecsv(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            int taille_tab_fichier = 0;
            byte[] tabnull = null;
            // Cette boucle nous permet d'avoir une idée de l'ordre de grandeur du futur tableau contenant les informations du fichier
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] != ';')
                        taille_tab_fichier++;
                }
            }
            byte[] fichier = new byte[taille_tab_fichier];

            StreamReader flux = null;
            string line;
            int k = 0;

            try
            {
                flux = new StreamReader(filename);
                while ((line = flux.ReadLine()) != null)
                {
                    string[] s = line.Split(';');
                    for (int j = 0; j < s.Length; j++)
                    {
                        if ((s[j].CompareTo("\0") != 0))            // si l'élément du tableau est vide alors cela correspond à la fin du ligne et il ne faut pas comptabiliser ces éléments 
                        {
                            fichier[k] = byte.Parse(s[j]);
                            k++;
                        }   
                    }
                }
                // On redimensionne la taille du tableau (fichierfinal) à la taille véritable du fichier (k) 
                byte[] fichierfinal = new byte[k];
                // ON remplit le fichierfinal avec les k premiers éléments non vides du tableau fichier 
                for (int i = 0; i < fichierfinal.Length; i++)
                {
                    fichierfinal[i] = fichier[i];
                }

            return fichierfinal;

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return tabnull;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                return tabnull;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return tabnull;
            }
            finally
            {   // Fermeture du flux dans tous les cas
                if (flux != null) { flux.Close(); }
            }
        }


        // On complète les données non sigificatives manquantes par ce qu'on veut, j'ai choisi de laisser des 0
        /// <summary>
        /// Convertit une instance de MonImage en fichier de type bmp ou csv
        /// </summary>
        /// <param name="file"> Nom du fihcier d'écriture (d'enregistrement) </param> 
        public void From_Image_To_File(string file)
        {
            byte[] myfile = new byte[54 + Image.Length*3]; //taille du header + taille du tableau de pixel*3 (car 1 pixel = 3 octets)

            // Header
            myfile[0] = 66;
            myfile[1] = 77;

            byte[] valeur = Convertir_Int_To_Endian(tailleFichier);
            for (int i = 2; i < 6; i++)
            {
                myfile[i] = valeur[i - 2];
            }

            valeur = Convertir_Int_To_Endian(tailleOffset);
            for (int i = 10; i < 14; i++)
            {
                myfile[i] = valeur[i - 10];
            }

            myfile[14] = 40;        // taille de l'entête sur 4 octets, vaut toujours 40

            valeur = Convertir_Int_To_Endian(largeurImage);
            for (int i = 18; i < 22; i++)
            {
                myfile[i] = valeur[i - 18];
            }

            valeur = Convertir_Int_To_Endian(hauteurImage);
            for (int i = 22; i < 26; i++)
            {
                myfile[i] = valeur[i - 22];
            }

            myfile[26] = 1;  //nombre de plans sur 2 octets, vaut toujours 1

            valeur = Convertir_Int_To_Endian(nbreBitsParCouleur);
            for (int i = 28; i < 30; i++)
            {
                myfile[i] = valeur[i - 28];
            }

            valeur = Convertir_Int_To_Endian(Image.Length*3);   // taille de l'image en octects sur 4 octets
            for (int i = 34; i < 38; i++)
            {
                myfile[i] = valeur[i - 34];
            }

            // Image
            int cpt = 54;
            for (int i=image.GetLength(0)-1; i >=0; i--)
            {
                for (int j =0; j < image.GetLength(1); j++)
                {
                    myfile[cpt] = (byte)image[i, j].Bleu;
                    myfile[cpt + 1] = (byte)image[i, j].Vert;
                    myfile[cpt + 2] = (byte)image[i, j].Rouge;
                    cpt+=3;
                }
            }

            

            if (file[file.Length - 1].CompareTo('v') == 0)      // si le nom du fichier d'enregistrement se termine par un v, le format d'enregistrement est du csv
            {
                EcrireFichierCsv(file, myfile);
            }
            else if (file[file.Length - 1].CompareTo('p') == 0)     // si le nom du fichier d'enregistrement se termine par p, c'est du bmp
            {
                File.WriteAllBytes(file, myfile);
                Process.Start(file);
            }
            else { Console.WriteLine("Extension du fichier de sortie incorrect"); }         
            
        }

        /// <summary>
        /// Complément de la fonction From_Image_To_File dans le cas particulier d'un fichier csv
        /// </summary>
        /// <param name="filename"> Nom du fichier d'écriture </param> 
        /// <param name="myfile"> Tableau contenant les informations de l'image en littleEndian </param> 
        public void EcrireFichierCsv ( string filename, byte [] myfile)
        {
            StreamWriter str = new StreamWriter(filename);
            try
            {
                string l = "";
                // La première ligne du fichier correspond au header
                for (int i = 0; i < 14; i++)
                {
                    l += myfile[i];
                    if (i < 13) l += ";";
                    else l += "\r\n";
                }
                str.Write(l);

                // La seconde ligne du fichier correspond au header info
                l = "";
                for (int i = 14; i < 54; i++)
                {
                    l += myfile[i];
                    if (i < 53) l += ";";
                    else l += "\r\n";
                }
                str.Write(l);

                // Le reste du tableau correspond à l'image en tant que telle
                int compteur = 54;
                int longueur;
                while (compteur < myfile.Length)
                {
                    l = "";
                    longueur = 0;
                    while (longueur < 15)       // On prévoit des lignes dans le futur fichier contenant au maximum 15 éléments du tableau pour une meilleure visibilité
                    {
                        l += myfile[compteur];
                        if (longueur < 14)
                        {
                            l += ";";
                        }
                        else l += "\r\n";
                        longueur++;
                        compteur++;
                    }
                    str.Write(l);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (str != null) { str.Close(); }
            }
            Process.Start(filename);

        }

        /// <summary>
        /// Convertit un tableau de bytes en entier
        /// </summary>
        /// <param name="tab"> Tableau de bytes à convertir </param>
        /// <returns> Entier converti </returns>
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int val = 0;
            if (tab != null && tab.Length == 4)
            {
                // On positionne les bytes de tab dans val dans l'ordre décroissant pour prendre compte de l'ordre inversé du little Endian
                // Ainsi par exemple, tab[3] est déplacé de 24 bits vers la gauche donc prend la position de tab[0]
                val = tab[3] << 24 | tab[2] << 16 | tab[1] << 8 | tab[0] << 0;
            }
            return val;
        }

        /// <summary>
        /// Convertit un entier en tableau de bytes
        /// </summary>
        /// <param name="val"> Entier à convertir </param>
        /// <returns>TTableaux de bytes converti </returns>
        public byte[] Convertir_Int_To_Endian(int val)
        {
            byte[] result = null;
            if (val > 0)
            {
                result = new byte[4];
                // On affecte les valeurs au tableau dans l'ordre décroissant afin de tenir compte du format little Endian
                result[3] = (byte)(val >> 24);        // On déplace les bits de val vers la droite de 24 position, on untilise un cast (byte) pour ne garder que les 8 premiers bits
                result[2] = (byte)(val >> 16);
                result[1] = (byte)(val >> 8);
                result[0] = (byte)(val);

            }
            else if (val == 0)
            {
                result = new byte[] { 0, 0, 0, 0 };
            }
            return result;
        }

        /// <summary>
        /// Permet de créer une chaine contenant les informations intéressantes du header de l'image
        /// </summary>
        /// <returns> Chaine de type string contenant ces informations </returns>
        public string toString()
        {
            string rep = "";
            rep += "\n Informations \n";
            rep += "Type d'image : " + typeImage + "\nTaille du fichier : " + tailleFichier + "\nTaille de l'Offset : " + tailleOffset + "\nHauteur de l'image : " + hauteurImage + "\nLargueur de l'image : " + largeurImage + "\nNombre de bits par couleur : " + nbreBitsParCouleur;
            return rep;
        }

        /// <summary>
        /// Transforme l'image en niveaux de gris en appliquant à chaque pixel de l'image, la méthode NiveauxDeGris() de la classe Pixel
        /// </summary>
        public void NiveauxGris()
        {
            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < LargeurImage; j++)
                {
                    image[i, j].NiveauxDeGris();
                }
            }
        }

        /// <summary>
        /// Transforme l'image en Noir et Blanc en appliquant à chaque pixel de l'image, la méthode NoirEtBlanc() de la classe Pixel
        /// </summary>
        public void NoirBlanc()
        {
            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    image[i, j].NoirEtBlanc();
                }
            }
        }

        /// <summary>
        /// Effectue une rotation à 180 degrés de l'image
        /// </summary>
        public void Rotation_180()
        {
            // Création d'une matrice miroir permettant de garder une trace de la matrice de départ
            Pixel[,] miroir = new Pixel[hauteurImage, largeurImage];

            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    miroir[i, j] = image[i, j];
                }
            }
            // rotation 180 degrés
            for (int i = 0; i < miroir.GetLength(0); i++)
            {
                for (int j = 0; j < miroir.GetLength(1); j++)
                {
                    image[i, j] = miroir[hauteurImage - 1 - i, largeurImage - 1 - j];
                }
            }
        }

        /// <summary>
        /// Effectue une rotation à 90 degrés de l'image (dans le sens anti-horaire)
        /// </summary>
        public void Rotation_90()
        {
            Pixel[,] miroir = new Pixel[largeurImage, hauteurImage];
            // Rotation 90 degrés
            for (int i = 0; i < miroir.GetLength(0); i++)
            {
                for (int j = 0; j < miroir.GetLength(1); j++)
                {
                    miroir[i, j] = image[j, largeurImage - 1 - i];
                }
            }
            
            // Ne pas oublier de redimensionner les attributs pour l'écriture du fichier
            hauteurImage = miroir.GetLength(0);
            largeurImage = miroir.GetLength(1);
            image = miroir;
        }

        /// <summary>
        /// Effectue une rotation à 270 degrés de l'image (dans le sens anti-horaire)
        /// </summary>
        public void Rotation_270()
        {
            Pixel[,] image1 = new Pixel[largeurImage, hauteurImage];
            for (int i = 0; i < image1.GetLength(0); i++)
            {
                for (int j = 0; j < image1.GetLength(1); j++)
                {
                    image1[i, j] = image[hauteurImage - 1 - j, i];
                }
            }
            
            // Ne pas oublier de redimensionner les attributs pour l'écriture du fichier
            hauteurImage = image1.GetLength(0);
            largeurImage = image1.GetLength(1);
            image = image1;
        }

        /// <summary>
        /// Transforme l'image en son reflet dans un miroir
        /// </summary>
        public void Miroir()
        {
            // Création d'une matrice miroir permettant de garder une trace de la matrice de départ
            Pixel[,] miroir = new Pixel[hauteurImage, largeurImage];

            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    miroir[i, j] = image[i, j];
                }
            }

            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    image[i, j] = miroir[i, largeurImage - 1 - j];
                }
            }
        }

        /// <summary>
        /// Permmet d'agrandir une image
        /// </summary>
        /// <param name="facteur"> Supérieur à 1, c'est le facteur qui multiplie les dimensions de l'image d'origine pour obtenir celles de l'image d'arrivée </param> 
        public void Agrandir(double facteur)
        {
            Pixel[,] resultat = null;
            if (facteur > 0)
            {
                int hauteur = (int)(hauteurImage * facteur);
                int largueur = (int)(largeurImage * facteur);
                resultat = new Pixel[hauteur, largueur];
                int increment = (int)facteur;
                // Si le facteur n'est pas un entier, l'incrément est augmenté de 1
                if ((facteur * 10) % 10 != 0) increment += 1;
                
                int ligne = 0;
                int colonne = 0;
                for (int i = 0; i < HauteurImage; i++)
                {
                    colonne = 0;
                    for (int j = 0; j < largeurImage; j++)
                    {
                        for (int l = ligne; (l < ligne + increment) && (l < resultat.GetLength(0)); l++)
                        {
                            for (int c = colonne; (c < colonne + increment) && (c < resultat.GetLength(1)); c++)
                            {
                                resultat[l, c] = image[i, j];
                            }
                        }
                        colonne += increment;
                    }
                    ligne += increment;
                }
            }
            image = resultat;
            hauteurImage = resultat.GetLength(0);
            largeurImage = resultat.GetLength(1);
            tailleFichier = hauteurImage * largeurImage * 3 + 54;
        }

        /// <summary>
        /// Permet de rétrécir une image
        /// </summary>
        /// <param name="facteur"> Supérieur à 0 et diviseur commun aux deux dimensions de l'image, c'est le facteur qui divise les dimensions de l'image d'origine pour obtenir celles de l'image d'arrivée </param>
        public void Retrecir (double facteur)
        {
            Pixel[,] resultat = null;
            if (facteur > 0 && (hauteurImage % facteur == 0) && (largeurImage % facteur == 0))
            {
                int largeur = (int)(largeurImage / facteur);    
                int hauteur = (int)(hauteurImage / facteur);   
                int increment = (int)facteur;  

                resultat = new Pixel[hauteur, largeur];
                int rouge = 0;
                int bleu = 0;
                int vert = 0;

                for (int ligne = 0; ligne < hauteurImage; ligne += increment)
                {
                    for (int colonne = 0; colonne < largeurImage; colonne += increment)
                    {
                        for (int l = ligne; (l < ligne + increment) && (l < hauteurImage); l++)
                        {
                            for (int c = colonne; (c < colonne + increment) && (c < largeurImage); c++)
                            {
                                rouge += image[l, c].Rouge;
                                vert += image[l, c].Vert;
                                bleu += image[l, c].Bleu;
                            }
                        }
                        
                        // On vérifie que les indices ne dépassent pas les limites de la matrice
                        if ((ligne / increment < hauteur) && (colonne / increment < largeur))
                        {
                            resultat[ligne / increment, colonne / increment] = new Pixel((int)rouge / (increment * increment), (int)vert / (increment * increment), (int)bleu / (increment * increment)); // increment * increment représente le nombre de pixels parcourus pour incrémenter les valeurs des variables rouge, vert, bleu

                        }
                        
                        rouge = 0;
                        vert = 0;
                        bleu = 0;
                    }
                }
                image = resultat;
                hauteurImage = resultat.GetLength(0);
                largeurImage = resultat.GetLength(1);
                tailleFichier = hauteurImage * largeurImage * 3 + 54;
            }
            else
            {
                Console.WriteLine(" Le facteur de rétrécissement, n'est pas strictement positif ou n'est pas un diviseur commun aux deux dimensions de l'image ");
            }
        }

        /// <summary>
        /// Permet d'appliquer un filtre à l'image
        /// </summary>
        /// <param name="mat_convolution"> Matrice de convolution correspondant au filtre choisi </param> 
        /// <param name="flou"> Si le paramètre == true alors le filtre choisi est le flou et il faut diviser les nouvelles valeurs des pixels par 9 </param>
        /// <returns> Matrice de pixels représentant l'image filtrée </returns>
        public Pixel[,] Filtre(int[,] mat_convolution, bool flou)
        {
            Pixel[,] resultat = new Pixel[hauteurImage, largeurImage];
            int sommeB = 0;
            int sommeV = 0;
            int sommeR = 0;

            // On parcourtla matrice de pixel en partant du 2eme élément jusqu'à l'avant dernier en ligne et en colonne car les pixels correspondants ne possèdent pas 9 voisins
            for (int i = 1; i < resultat.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < resultat.GetLength(1) - 1; j++)
                {
                    // Pour chaque pixel de l'image, on parcourt le carré de 9 pixels le comprenant
                    for (int ligne = i - 1; ligne <= i + 1; ligne++)
                    {
                        for (int colonne = j - 1; colonne <= j + 1; colonne++)
                        {
                            // On multiplie la valeur de chaque composante de chaque pixel par la valeur correspondante dans la matrice de convolution
                            sommeB += image[ligne, colonne].Bleu * mat_convolution[ligne - i + 1, colonne - j + 1];
                            sommeV += image[ligne, colonne].Vert * mat_convolution[ligne - i + 1, colonne - j + 1];
                            sommeR += image[ligne, colonne].Rouge * mat_convolution[ligne - i + 1, colonne - j + 1];
                        }
                    }

                    // pour le flou uniquement (il faut faire attention à la somme des coeff de la mat_convolution, il faut diviser par la somme des coeff)
                    if (flou)
                    {
                        sommeR = sommeR / 9;
                        sommeB = sommeB / 9;
                        sommeV = sommeV / 9;
                    }

                    resultat[i, j] = new Pixel(sommeR, sommeV, sommeB);
                    sommeB = 0;
                    sommeR = 0;
                    sommeV = 0;
                }
            }

            // On complète maintenant les bords de l'image par extention des pixels voisins
            for (int j = 1; j < resultat.GetLength(1) - 1; j++)         // Remplissage des 2 lignes en bordure
            {
                resultat[0, j] = resultat[1, j];
                resultat[resultat.GetLength(0) - 1, j] = resultat[resultat.GetLength(0) - 2, j];
            }
            for (int i = 0; i < resultat.GetLength(0); i++)             // Remplissage des 2 colonnes en bordure
            {
                resultat[i, 0] = resultat[i, 1];
                resultat[i, resultat.GetLength(1) - 1] = resultat[i, resultat.GetLength(0) - 2];
            }

            return resultat;
        }

        /// <summary>
        /// Permet de "normaliser" une image en niveaux de gris en "étalant" son histogramme
        /// </summary>
        /// <param name="histo">Instance de la classe Histogramme qui permet d'avoir accès notamment à l'histogramme général (toutes composantes confondues) de l'image</param>
        public void Normalisation(Histogramme histo)   
        {
            for (int ligne = 0; ligne < hauteurImage; ligne++)
            {
                for (int colonne = 0; colonne < largeurImage; colonne++)
                {
                    // On applique la formule de normalisation à chaque composante du pixel
                    image[ligne, colonne].Bleu = (int)((image[ligne, colonne].Bleu - histo.MinNG) * 255 / (histo.MaxNG - histo.MinNG));
                    image[ligne, colonne].Rouge = (int)((image[ligne, colonne].Rouge - histo.MinNG) * 255 / (histo.MaxNG - histo.MinNG)); ;
                    image[ligne, colonne].Vert = (int)((image[ligne, colonne].Vert - histo.MinNG) * 255 / (histo.MaxNG - histo.MinNG)); ;
                }
            }
        }
    }
}
