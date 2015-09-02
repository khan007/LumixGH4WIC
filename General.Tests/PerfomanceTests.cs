﻿using System;
using System.Diagnostics;
using System.IO;
using com.azi.Decoder.Panasonic.Rw2;
using com.azi.Filters;
using com.azi.Filters.VectorMapFilters;
using com.azi.Filters.Converters.Demosaic;
using com.azi.Image;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.azi.Filters.Converters;

namespace General.Tests
{
    [TestClass]
    public class PerfomanceTests
    {
        [TestMethod]
        public void FullProcessTest()
        {
            var stopwatch = Stopwatch.StartNew();
            const int maxIter = 5;
            for (var iter = 0; iter < maxIter; iter++)
            {
                Stream stream = new FileStream(@"..\..\..\PanasonicRW2.Tests\P1350577.RW2", FileMode.Open,
                    FileAccess.Read);
                var decoder = new PanasonicRW2Decoder();
                var exif = decoder.DecodeExif(stream);
                var raw = decoder.DecodeMap(stream, exif);
                var debayer = new AverageBGGRDemosaic();
                var white = new WhiteBalanceFilter();
                //white.AutoAdjust(color16Image);
                var gamma = new GammaFilter();


                var light = new LightFilter();
                //light.AutoAdjust(color16Image);

                var compressor = new VectorRGBCompressorFilter();
                var pipeline = new FiltersPipeline(new IFilter[]
                {
                    debayer,
                    white,
                    gamma,
                    light,
                    compressor
                });
                pipeline.ProcessFilters(raw);
            }
            stopwatch.Stop();
            Console.WriteLine("FullProcess: " + stopwatch.ElapsedMilliseconds / maxIter + "ms");

            //Before Curve - Release 3756ms
            //After Curve - Release 1900ms
            //2015 - 409ms
        }

        [TestMethod]
        public void FullProcessP1460461Test()
        {
            var stopwatch = Stopwatch.StartNew();
            const int maxIter = 1;
            for (var iter = 0; iter < maxIter; iter++)
            {
                Stream stream = new FileStream(@"..\..\..\PanasonicRW2.Tests\P1460461.RW2", FileMode.Open,
                    FileAccess.Read);
                var decoder = new PanasonicRW2Decoder();
                var exif = decoder.DecodeExif(stream);
                var raw = decoder.DecodeMap(stream, exif);
                var debayer = new AverageBGGRDemosaic();

                var white = new WhiteBalanceFilter();
                //white.AutoAdjust(color16Image);
                var gamma = new GammaFilter();


                var light = new LightFilter();
                //light.AutoAdjust(color16Image);

                var compressor = new VectorRGBCompressorFilter();
                var pipeline = new FiltersPipeline(new IFilter[]
                {
                    debayer,
                    white,
                    gamma,
                    light,
                    compressor
                });
                pipeline.ProcessFilters(raw);
            }
            stopwatch.Stop();
            Console.WriteLine("FullProcess: " + stopwatch.ElapsedMilliseconds / maxIter + "ms");

            //Before Curve - Release 3756ms
            //After Curve - Release 1900ms
        }


        [TestMethod]
        public void FullProcessWithAutoAdjustTest()
        {
            var stopwatch = Stopwatch.StartNew();
            const int maxIter = 5;
            for (var iter = 0; iter < maxIter; iter++)
            {
                Stream stream = new FileStream(@"..\..\..\PanasonicRW2.Tests\P1350577.RW2", FileMode.Open,
                    FileAccess.Read);
                var debayer = new AverageBGGRDemosaic();

                var white = new WhiteBalanceFilter();
                //white.AutoAdjust(color16Image);
                var gamma = new GammaFilter();


                var light = new LightFilter();
                //light.AutoAdjust(color16Image);

                var compressor = new VectorRGBCompressorFilter();
                var filters = new IFilter[]
                {
                    debayer,
                    white,
                    gamma,
                    light,
                    compressor
                };

                var decoder = new PanasonicRW2Decoder();
                var exif = decoder.DecodeExif(stream);

                var processor = new ImageProcessor(decoder.DecodeMap(stream, exif), filters);

                processor.Invoke();
            }
            stopwatch.Stop();
            Console.WriteLine("FullProcessWithAutoAdjust: " + stopwatch.ElapsedMilliseconds / maxIter + "ms");

            //Before Curve - Release 3756ms
            //After Curve - Release 1900ms
            //2015 Vector3 - Release 1305ms
        }
    }
}