using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox.General
{

    public class TickerSystem : Singleton<TickerSystem>
    {
        private List<Ticker> tickers = new List<Ticker>();

        public Ticker Create(float maxValue) {
            Ticker ticker = new Ticker(maxValue);
            tickers.Add(ticker);
            return ticker;
        }

        void Update() {
            foreach (Ticker ticker in tickers) {
              //cker.Update();
            }
        }
    }
}