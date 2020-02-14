using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;

namespace ReproductordeAudio
{
    class FadeIn : ISampleProvider
    {

        private ISampleProvider fuente;
        private int muestrasLeidas = 0;
        private float segundosTranscurridos = 0;
        private float duracion;
        public FadeIn(ISampleProvider fuente, float duracion)
        {
            this.fuente = fuente;
            this.duracion = duracion;
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
            int read = fuente.Read(buffer, offset, count);
            //Fade In 
            //Para calcular tiempo trans.
            muestrasLeidas += read;

            //para no tener problemas con enteros
            segundosTranscurridos = ((float)(muestrasLeidas) /(float)( fuente.WaveFormat.SampleRate))
                /(float)(fuente.WaveFormat.Channels);
            
            if(segundosTranscurridos <= duracion)
            {
                /**/
                //aplicar efecto
                float factorEscala = segundosTranscurridos / duracion;
                for (int i=0; i <read; i++)
                {//*= escalar
                    buffer[i + offset] *= factorEscala;
                }
            }


            return read;
        }
    }
}
