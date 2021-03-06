﻿/* ------------------------------------------------------------------------- */
///
/// Monitoring/ShikokuClientTester.cs
///
/// Copyright (c) 2013 CubeSoft, Inc. All rights reserved.
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program.  If not, see < http://www.gnu.org/licenses/ >.
///
/* ------------------------------------------------------------------------- */
using System;
using NUnit.Framework;

namespace CubePowerTests.Monitoring
{
    /* --------------------------------------------------------------------- */
    ///
    /// ShikokuClientTester
    /// 
    /// <summary>
    /// ShikokuClient クラスをテストするためのクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    [TestFixture]
    public class ShikokuClientTester
    {
        /* ----------------------------------------------------------------- */
        ///
        /// TestFromFile
        /// 
        /// <summary>
        /// 電力情報を取得するためのテストです。このテストでは、サーバへ
        /// 問い合わせる代わりに、予め保存しているファイルを読み込んで
        /// テストします。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void TestFromFile()
        {
            try
            {
                var client = new CubePower.Monitoring.ShikokuClient();
                Assert.AreEqual(CubePower.Monitoring.Area.Shikoku, client.Area);

                var filename = "shikoku.csv";
                var time = new DateTime(2013, 6, 4, 00, 00, 00);
                var response = GetResponseHelper.Run(client, filename, time);
                Assert.NotNull(response);
                Assert.AreEqual(2013, response.Time.Year);
                Assert.AreEqual(6, response.Time.Month);
                Assert.AreEqual(4, response.Time.Day);
                Assert.AreEqual(0, response.Time.Hour);
                Assert.AreEqual(0, response.Time.Minute);
                Assert.AreEqual(0, response.Time.Second);
                Assert.AreEqual("万kW", response.Unit);
                Assert.AreEqual(439, response.Capacity);
                Assert.AreEqual(270, response.Usage);
                Assert.AreEqual(62, response.UsageRatio);

                time = new DateTime(2013, 6, 4, 12, 08, 00);
                response = GetResponseHelper.Run(client, filename, time);
                Assert.NotNull(response);
                Assert.AreEqual(2013, response.Time.Year);
                Assert.AreEqual(6, response.Time.Month);
                Assert.AreEqual(4, response.Time.Day);
                Assert.AreEqual(12, response.Time.Hour);
                Assert.AreEqual(6, response.Time.Minute);
                Assert.AreEqual(0, response.Time.Second);
                Assert.AreEqual("万kW", response.Unit);
                Assert.AreEqual(439, response.Capacity);
                Assert.AreEqual(352, response.Usage);
                Assert.AreEqual(80, response.UsageRatio);

                time = new DateTime(2013, 6, 3, 12, 08, 00);
                response = GetResponseHelper.Run(client, filename, time);
                Assert.IsNull(response);
            }
            catch (Exception err) { Assert.Fail(err.ToString()); }
        }

        /* ----------------------------------------------------------------- */
        ///
        /// TestFromServer
        /// 
        /// <summary>
        /// サーバへ問い合わせて電力情報を取得するためのテストです。
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        [Test]
        public void TestFromServer()
        {
            try
            {
                var client = new CubePower.Monitoring.ShikokuClient();
                Assert.AreEqual(CubePower.Monitoring.Area.Shikoku, client.Area);

                var response = client.GetResponse(DateTime.Now);
                Assert.NotNull(response);
            }
            catch (Exception err) { Assert.Fail(err.ToString()); }
        }
    }
}
