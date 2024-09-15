using System;
using System.Threading;
using System.Threading.Tasks;

namespace Quiztle.API.LocalLLM
{
    public class Runtime
    {
        private CancellationTokenSource _cancellationTokenSource;
        private int _interval;
        private readonly object _lock = new object();

        public Runtime()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _interval = 10000;
        }

        public async Task Start()
        {
            await Task.Run(() => PrintHelloWorldPeriodically(_cancellationTokenSource.Token));
        }

        public void Stop()
        {
            // Cancela a tarefa quando parar
            _cancellationTokenSource.Cancel();
        }

        public void UpdateInterval(int newInterval)
        {
            lock (_lock)
            {
                _interval = newInterval;
            }
        }

        private async Task PrintHelloWorldPeriodically(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("hello world in " + _interval);

                // Obtém o intervalo atual com segurança
                int currentInterval;
                lock (_lock)
                {
                    currentInterval = _interval;
                }

                // Espera pelo intervalo especificado
                await Task.Delay(currentInterval, cancellationToken);
            }
        }
    }
}
