﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Analytics;
using CardsBlazor.ApiControllers;
using CardsBlazor.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace CardsBlazor.Data
{
    public class PlayerService
    {
        private readonly CardsAppContext _context;
        private readonly IAnalytics _analytic;
        public PlayerService(CardsAppContext context, IAnalytics analytic)
        {
            _context = context;
            _analytic = analytic;
        }

        public Player GetPlayer(int playerId)
        {
            return _context.Players.Find(playerId);
        }

        public async Task<List<Player>> GetPlayers()
        {
            return await _context.Players.OrderBy(x => x.PlayerId).ToListAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Gets all of the players in the form a view model
        /// </summary>
        /// <returns></returns>
        public async Task<List<PlayerViewModel>> GetPlayersViewModel()
        {
            return await _context.Players.Include(x => x.MatchesParticipatedIn).ThenInclude(x => x.Match).Where(x => !x.Archived && !x.HideFromView).OrderBy(x => x.PlayerId).Select(x => new PlayerViewModel(x)).ToListAsync().ConfigureAwait(true);
        }

        public IQueryable<Player> GetAllAsQueryable()
        {
            return _context.Players.Where(x => !x.Archived)
                .Include(x => x.MatchesParticipatedIn).ThenInclude(x => x.Match)
                .Include(x => x.CashGamesPlayed).ThenInclude(x => x.Game)
                .AsQueryable();
        }

        public List<PlayerApiModel> GetAllPlayersAsApiModel()
        {
             return _context.Players.Where(x => !x.Archived)
                .Include(x => x.MatchesParticipatedIn).ThenInclude(x => x.Match)
                .Include(x => x.CashGamesPlayed).ThenInclude(x => x.Game)
                .AsSplitQuery().Select(x => new PlayerApiModel()
                {
                    Id = x.PlayerId,
                    UserName = x.UserName,
                    RealName = x.RealName,
                    CurrentPosition = x.CurrentPosition,
                    PotentialPosition = x.PotentialPosition,
                    HideFromView = x.HideFromView
                }).ToList();
        }

        public void AddPlayer(PlayerViewModel player)
        {
            var model = new Player
            {
                UserName = player.UserName,
                EmailAddress = player.EmailAddress,
                Archived = false,
                HasAdminPermission = player.HasAdminPermission,
                RealName = player.RealName,
                LastPaid = DateTime.Now
            };

            _context.Players.Add(model);
            _context.SaveChanges();
            _analytic.TrackEvent("Added Player", model.PlayerId.ToString(CultureInfo.InvariantCulture));
        }

        public List<PositionGraphClass> GetPositionGraphClasses(int playerId, int differenceBetweenPoints)
        {
            var returnable = new List<PositionGraphClass>();
            var player = _context.Players
                .Include(x => x.MatchesParticipatedIn)
                .ThenInclude(x => x.Match)
                .First(x => x.PlayerId == playerId);
            var startDate = getStartOfFinancialQtr(DateTime.Now, 1);
            var totalWeeks = Convert.ToInt32(Math.Ceiling((DateTime.Now - startDate).TotalDays / differenceBetweenPoints));
            var partiesToSearch = player.MatchesParticipatedIn.Where(x => x.IsResolved &&
                x.Match.EndTime.HasValue && x.Match.EndTime.Value.Date >= startDate).ToList();
            for (int i = 0; i < totalWeeks; i++)
            {
                var dateToFind = DateTime.Now.AddDays(-i * differenceBetweenPoints);
                var parties = partiesToSearch.Where(x =>
                    x.Match.EndTime.HasValue && x.Match.EndTime.Value.Date <= dateToFind).ToList();
                var returnObj = new PositionGraphClass
                {
                    PositionAtTime = Convert.ToDecimal(parties.Sum(x => x.NetResult)),
                    WeekDate = dateToFind
                };
                returnable.Add(returnObj);
            }

            return returnable;
        }
        private DateTime getStartOfFinancialQtr(DateTime date, int monthFinancialYearStartsOn)
        {
            var actualMonth = date.Month;
            var financialYear = date.Year;
            var difference = actualMonth - monthFinancialYearStartsOn;
            if (difference < 0)
            {
                --financialYear;
                difference += 12;
            }
            var quarter = difference / 3;

            return new DateTime(financialYear, monthFinancialYearStartsOn, 1).AddMonths(quarter * 3);
        }
    }

    public class PositionGraphClass
    {
        [DisplayName("Date")]
        public DateTime WeekDate { get; set; }
        [DisplayName("Position")]
        public decimal PositionAtTime { get; set; }
    }
}
