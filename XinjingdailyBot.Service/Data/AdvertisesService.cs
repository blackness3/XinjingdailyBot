﻿using XinjingdailyBot.Infrastructure.Attribute;
using XinjingdailyBot.Interface.Data;
using XinjingdailyBot.Model.Models;

namespace XinjingdailyBot.Service.Data
{
    [AppService(typeof(IAdvertisesService), LifeTime.Transient)]
    public sealed class AdvertisesService : BaseService<Advertises>, IAdvertisesService
    {
        public async Task<Advertises?> GetPostableAdvertise()
        {
            var ads = await Queryable().Where(x => x.Enable).ToListAsync();

            var now = DateTime.Now;
            //检查是否过期
            foreach (var ad in ads)
            {
                if ((ad.MaxShowCount > 0 && ad.ShowCount >= ad.MaxShowCount) ||
                    now >= ad.ExpireAt ||
                    ad.Weight == 0)
                {
                    ad.Enable = false;
                    await UpdateAsync(ad);
                }
            }
            var validAds = ads.Where(x => x.Enable);

            if (!validAds.Any())
            {
                return null;
            }

            //随机抽取广告
            int sum = 0;
            var weightList = new List<(int, Advertises)>();

            foreach (var ad in validAds)
            {
                sum += ad.Weight;
                weightList.Add(new(sum, ad));
            }

            int randInt = new Random().Next(0, sum);

            var randomAd = weightList.First(kv => kv.Item1 > randInt).Item2;

            return randomAd;
        }
    }
}
