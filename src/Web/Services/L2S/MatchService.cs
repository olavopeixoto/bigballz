﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq;
using System.Linq;
using BigBallz.Core;
using BigBallz.Infrastructure;
using BigBallz.Models;

namespace BigBallz.Services.L2S
{
    public class MatchService : IMatchService, IDisposable
    {
        private readonly DataContextProvider _provider;
        private BigBallzDataContext _context;

        public MatchService(DataContextProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<Match> GetAll()
        {
            //var options = new DataLoadOptions();
            //options.LoadWith<Match>(x => x.Team1Id);
            //options.LoadWith<Match>(x => x.Team2Id);
            //options.LoadWith<Match>(x => x.StageId);
            //options.LoadWith<Team>(x => x.GroupId);

            using (var db = _provider.CreateContext())
            {
                //db.LoadOptions = options;

                return db.Matches
                    .Include(x => x.Team1Obj)
                    .Include(x => x.Team2Obj)
                    .Include(x => x.Stage1)
                    .ToList();
            }
        }

        public Match Get(int id)
        {
            if (_context == null)
            {
                _context = _provider.CreateContext();
            }

            return _context.Matches
                        .Include(x => x.Team1Obj)
                        .Include(x => x.Team2Obj)
                        .Include(x => x.Stage1)
                        .SingleOrDefault(d => d.MatchId == id);
        }

        public IEnumerable<Match> GetNextMatches()
        {
            //var options = new DataLoadOptions();
            //options.LoadWith<Match>(x => x.Team1Id);
            //options.LoadWith<Match>(x => x.Team2Id);

            var now = DateTime.Now.BrazilTimeZone();

            using (var db = _provider.CreateContext())
            {
                //db.LoadOptions = options;

                return db.Matches.Where(x => x.StartTime > now)
                        .Take(5)
                        .OrderBy(x => x.StartTime)
                        .Include(x => x.Team1Obj)
                        .Include(x => x.Team2Obj)
                        .ToList();
            }
        }

        public IEnumerable<Match> GetLastPlayedMatches()
        {
            //var options = new DataLoadOptions();
            //options.LoadWith<Match>(x => x.Team1Id);
            //options.LoadWith<Match>(x => x.Team2Id);

            var now = DateTime.Now.BrazilTimeZone();

            using (var db = _provider.CreateContext())
            {
                return db.Matches
                            .Where(x => x.StartTime < now)
                            .OrderByDescending(x => x.StartTime)
                            .Include(x => x.Team1Obj)
                            .Include(x => x.Team2Obj)
                            .Take(5)
                            .ToList();
            }
        }

        public void Add(Match match)
        {
            using (var db = _provider.CreateContext())
            {
                db.Matches.Add(match);
                db.SaveChanges();
                AlertEndBetTask.AddTask(match.StartTime.AddHours(-1));
                BetExpirationWarningTask.AddTask(match.StartTime.AddHours(-2));
            }
        }

        public void Delete(Match match)
        {
            if (_context == null) return;

            _context.Matches.Remove(match);
        }

        public void Save()
        {
            if (_context == null) return;

            _context.SaveChanges();
        }

        public DateTime GetStartDate()
        {
            using (var db = _provider.CreateContext())
            {
                return db.Matches.Min(x => (DateTime?) x.StartTime) ?? DateTime.Now;
            }
        }

        public void Dispose()
        {
            if (_context!=null) _context.Dispose();
        }
    }
}