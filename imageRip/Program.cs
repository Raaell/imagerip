using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imageRip
{
    class Program
    {
        static void Main(string[] args)
        {
            var bmp = new Bitmap(@"C:\Users\ISRAEL\Desktop\NarutoUzumaki.png");

            int heigth = bmp.Height;

            int width = bmp.Width;

            var img2D = new Color[width * heigth];

            Console.WriteLine("Carregando Imagem2D para memória, Aguarde...");

            Console.ReadLine();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < heigth; y++)
                {
                    img2D[x * y] = bmp.GetPixel(x, y);

                    Console.WriteLine($"Carregando...    Estamos em {x * y} já processados...");
                }
            }



            var coresDistintas = img2D.Distinct().ToArray();

            var setoresImg = new ImageFrameworks.Setores { Filhos = new List<ImageFrameworks.Setores>(), Index = new List<int>() };

            Console.WriteLine("Separando imagem em Setores...");

            Console.ReadLine();

            for (int i = 0; i < coresDistintas.Length; i++)
            {
                var setorFilho = new ImageFrameworks.Setores { Filhos = new List<ImageFrameworks.Setores>(), Index = new List<int>() };
           
                var coresSeparadas = coresDistintas.Where(x => (coresDistintas[i].R - 30 <= x.R && x.R >= coresDistintas[i].R + 30) && (coresDistintas[i].G - 30 <= x.G && x.G >= coresDistintas[i].G + 30) && (coresDistintas[i].B - 30 <= x.B && x.B >= coresDistintas[i].B + 30)).ToArray();

                for (int j = 0; j < coresSeparadas.Length; j++)
                {
                    int vetor = coresDistintas.ToList().IndexOf(coresSeparadas[j]);

                    if (vetor.IsnewIndexin(setoresImg))
                    {
                        setorFilho.Index.Add(vetor);
                    }

                    Console.WriteLine($"Carregando...    {i * j} já processados...");
                }

                if (setorFilho.Index.Count > 0)
                {
                    setoresImg.Filhos.Add(setorFilho);
                }

                int indexes = setoresImg.GetAllIndex();

                if (coresDistintas.Length.Equals(indexes))
                {
                    break;
                }
            }

            return;
        }
    }

    public static class ImageFrameworks
    {
        public static byte[] ToArrayByte(this Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Bmp);
                return ms.ToArray();
            }
        }

        public static int GetAllIndex(this Setores Setor)
        {
            int indexes = Setor.Index.Count;            

            if (Setor.Filhos != null || Setor.Filhos.Count > 0)
            {
                for (int i = 0; i < Setor.Filhos.Count; i++)
                {
                    indexes += Setor.Filhos[i].GetAllIndex();
                }               
            }

            return indexes;
        }

        public static bool IsnewIndexin(this int vetor, Setores Setor)
        {
            try
            {
                if (Setor.Index.Contains(vetor))
                {
                    return false;
                }

                if (Setor.Filhos != null || Setor.Filhos.Count > 0)
                {
                    for (int i = 0; i < Setor.Filhos.Count; i++)
                    {
                        if (!vetor.IsnewIndexin(Setor.Filhos[i]))
                        {
                            return false;
                        }                        
                    }                    
                }

                return true;

            }
            catch (Exception)
            {
                throw;
            }            
        }

        public class Setores
        {
            public List<int> Index { get; set; } // index do array de cores

            public List<Setores> Filhos { get; set; }            
        }
    }
}