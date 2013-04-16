using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NGM.Wave.Models;
using Orchard.ContentManagement.Drivers;

namespace NGM.Wave.Drivers {
    [UsedImplicitly]
    public class WavePartDriver : ContentPartDriver<WavePart> {
        protected override string Prefix {
            get { return "WavePart"; }
        }

        protected override DriverResult Display(WavePart part, string displayType, dynamic shapeHelper) {
            List<DriverResult> results = new List<DriverResult>();

            results.Add(ContentShape("Parts_Wave_CurrentViewingUsers",
                                        () => shapeHelper.Parts_Wave_CurrentViewingUsers()));

            return Combined(results.ToArray());
        }

    }
}