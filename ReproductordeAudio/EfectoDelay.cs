﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;

namespace ReproductordeAudio
{
    class EfectoDelay : ISampleProvider
    {
        private ISampleProvider fuente;
        private int OffsetMiliSegundos;
        private float ganancia;
        public float Ganancia
        {
            get
            {
                return Ganancia;
            }
            set
            {
                if (value > 1)
                {
                    ganancia = 1.0f;
                }
                else if (value < 0)
                {
                    ganancia = 0.0f;
                }
                else
                {
                    ganancia = value;
                }
            }
        }

        public int OffsetMilisegundos
        {
            get
            {
                return OffsetMilisegundos;
            }
            set
            {
                if (value > 20000)
                {
                    OffsetMiliSegundos = 20000;
                }
                else if (value<0)
                {
                    OffsetMiliSegundos = 0;
                }
                else
                {
                    OffsetMiliSegundos = value;
                }
            }
        }

        List<float> muestras = new List<float>();
        private int tamañoBuffer;

        private int cantidadMuestrasTranscurridas = 0;
        private int cantidadmuestrasBorradas = 0;
        
        public EfectoDelay(ISampleProvider fuente, int offsetMiliSegundos, float ganancia)
        {
            this.fuente = fuente;
            this.OffsetMiliSegundos = offsetMiliSegundos;
            this.ganancia = ganancia;


            tamañoBuffer = (fuente.WaveFormat.SampleRate * 20) * fuente.WaveFormat.Channels;

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
            //Calculo de tiempos
            float milisegundosTranscurridos = (((float)cantidadMuestrasTranscurridas /
                ((float)(fuente.WaveFormat.SampleRate) *  (float)(fuente.WaveFormat.Channels)))) * 1000.0f;

            int numeroMuestrasOffset =(int)((OffsetMiliSegundos / 1000.0f)* (float)(fuente.WaveFormat.SampleRate));
            //Llenar el buffer
            for(int i=0; i < read ; i++)
            {
                muestras.Add(buffer[i + offset]);
            }
            //Si el buffer excede el tamaño, ajustar el número de elementos
            if(muestras.Count > tamañoBuffer)
            {
                //Eliminar el excedente 
                int diferencia = muestras.Count - tamañoBuffer;
                muestras.RemoveRange(0, diferencia);
                cantidadmuestrasBorradas += diferencia;
            }
            //Aplicar el efecto Delay
            if(milisegundosTranscurridos > OffsetMiliSegundos)
            {
                for (int i = 0; i < read; i++)
                {
                    buffer[i + offset] += ganancia *  muestras[(cantidadMuestrasTranscurridas -
                        cantidadmuestrasBorradas) + i - numeroMuestrasOffset];
                }
            }


            cantidadMuestrasTranscurridas += read; 
            return read;
        }
    }
}
