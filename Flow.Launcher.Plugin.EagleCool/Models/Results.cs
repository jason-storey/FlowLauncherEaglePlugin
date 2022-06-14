using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Launcher.Plugin.EagleCool
{
#pragma warning disable CS1591
    public class Results : IEnumerable<Result>
    {
        List<Result> _results;

        public Results()
        {
            _results = new List<Result>();
        }

        public bool HasEntries => _results.Count > 0;

        public void Add(string title)
        {
            _results.Add(new Result{ Title = title});
        }

        public void AddAll(params string[] entries)
        {
            foreach (var entry in entries) Add(entry);
        }
        
        public void Add(string title, string subtitle)
        {
            _results.Add(new Result{ Title = title,SubTitle = subtitle});
        }

        public void Add(Result result) => _results.Add(result);

        public void Add(string title, string subtitle, Action action)
        {
            _results.Add(new Result{ Title = title,SubTitle = subtitle, Action = x =>
            {
                action?.Invoke();
                return true;
            }});
        }
        
        public void Add(string title, string subtitle,string icon, Action action)
        {
            _results.Add(new Result{ Title = title,SubTitle = subtitle,IcoPath = icon,Action = x =>
            {
                action?.Invoke();
                return true;
            }});
        }

        public Results(IEnumerable<Result> results) => _results = results.ToList();


        public IEnumerator<Result> GetEnumerator() => _results.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static implicit operator List<Result>(Results results) => results.GetAll();
        public static implicit operator Results(List<Result> results) => Results.From(results);

        static Results From(List<Result> results) => new Results(results);

        List<Result> GetAll() => _results.ToList();

        public void CreateFrom<T>(List<T> items, Action<T,Result> action)
        {
            List<Result> newResults = new List<Result>();
            foreach (var item in items)
            {
                var result = new Result();
                action?.Invoke(item,result);
                newResults.Add(result);
            }
            _results.AddRange(newResults);
        }

        public static List<Result> Create<T>(List<T> items, Action<T,Result> action)
        {
            var results = new Results();
            results.CreateFrom(items,action);
            return results;
        }

        public void AddRange(IEnumerable<Result> results) => _results.AddRange(results);
    }
}