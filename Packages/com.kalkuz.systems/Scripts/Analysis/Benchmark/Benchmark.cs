using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Analysis
{
    /// <summary>
    /// Benchmark to measure how fast a method execution is.
    /// Create an instance at the start of a method then <see cref="EndBenchmark"/>
    /// to measure between.
    /// </summary>
    public class Benchmark
    {
        private readonly System.Diagnostics.Stopwatch stopwatch;

        /// <summary>
        /// Creates instance and <see cref="StartBenchmark"/>
        /// </summary>
        public Benchmark()
        {
            stopwatch = new System.Diagnostics.Stopwatch();
            StartBenchmark();
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartBenchmark()
        {
            stopwatch.Start();
        }

        /// <summary>
        /// Stops the timer and logs the time performance. 
        /// </summary>
        public void EndBenchmark()
        {
            stopwatch.Stop();
            Debugging.KalkuzLogger.Info($"Elapsed: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
