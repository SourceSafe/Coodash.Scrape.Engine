using Coodash.Scrape.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coodash.Scrape.Engine
{
    public class ScrapeEngine
    {
       
        private static ScrapeEngine _instance;
        public static ScrapeEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ScrapeEngine();
                }
                return _instance;
            }
        }

        private List<Worker> _workers = new List<Worker>();
        private BlockingCollection<IWorkerItem> _queue = new BlockingCollection<IWorkerItem>();


        public ScrapeEngine()
        {
            
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        }

        public void AddToQueue(IWorkerItem item)
        {
            lock (_queue)
            {
                _queue.Add(item);
            }
        }
        public void Start(int threadCount)
        {
            for (int i = 0; i < threadCount; i++)
                _workers.Add(new Worker(_queue));


            System.Diagnostics.Debug.WriteLine("Engine started with " + threadCount + " threads...");
        }
        public void Stop()
        {
            _workers.ForEach(worker => worker.Stop());

            System.Diagnostics.Debug.WriteLine("Engine stopped.");
        }
        

        public event NewsArticleParsedDelegate NewsArticleParsed;

        

        public void  OnNewsArticleParsedDelegate(ScrapedArticleArgs args)
        {
            NewsArticleParsed?.Invoke(args);
        }

       

    }

    public class Worker
    {
        private bool _running = true;
        private Thread _worker;
        private BlockingCollection<IWorkerItem> _queue = new BlockingCollection<IWorkerItem>();

        public void Stop() { _running = false; }

        public Worker(BlockingCollection<IWorkerItem> queue)
        {

            _running = true;
            _queue = queue;
            _worker = new Thread(Work);
            _worker.Start();
        }

        private void Work()
        {
            while (_running)
            {
                IWorkerItem item = _queue.Take();
                if (item != null)
                    item.Work();

                Thread.Sleep(3000);
            }

            System.Diagnostics.Debug.WriteLine("Thread stopped.");
        }
    }


   
}

