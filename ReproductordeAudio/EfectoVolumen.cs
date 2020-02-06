using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;
 

namespace ReproductordeAudio
{
    class EfectoVolumen : ISampleProvider// clase padre de todos los efectos
    {
        private ISampleProvider fuente;
        private float volumen;
        //constructor
        public EfectoVolumen(ISampleProvider fuente)
        {//this hace referencia a la clase. para no crear conflucto
            this.fuente = fuente;
            volumen = 0.2f;
        }

        public float Volumen
        {
            get
            {
                return volumen;
            }
            set
            {
                if (value < 0 )
                {
                    volumen = 0;
                }
                else if(value > 1)
                {
                    volumen = 1;
                }
                else
                {
                    volumen = value;
                }
            }
        }
        public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            int read = fuente.Read(buffer, offset,count);
            // Aplicar el efecto
            for(int i =0; i < read; i++)
            {
                buffer[i + offset] *= volumen;

            }
            return read;
        }
    }
}
