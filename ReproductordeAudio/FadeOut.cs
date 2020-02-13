using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;

namespace ReproductordeAudio
{
    class FadeOut : ISampleProvider
    {

        private ISampleProvider fuente;
        private int muestrasLeidas = 0;
        private float segundosTranscurridos = 0;
        private float duracion;
        private float inicio;
        public FadeOut(ISampleProvider fuente, float duracion, float inicio)
        {
            this.fuente = fuente;
            this.duracion = duracion;
            this.inicio = inicio;
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
            //Fade Out
            //Para calcular tiempo trans.
            muestrasLeidas += read;
            //para no tener problemas con enteros
            segundosTranscurridos = ((float)(muestrasLeidas) / (float)(fuente.WaveFormat.SampleRate))
                / (float)(fuente.WaveFormat.Channels);

            if (segundosTranscurridos >= inicio)
            {
                //aplicar efecto
                float factorEscala = 1 -  ((segundosTranscurridos - inicio) /duracion);
                for (int i = 1; i < read; i++)
                {//*= escalar
                    buffer[i + offset] *= factorEscala;
                }
            }
            
            return read;
        }
    }
}
