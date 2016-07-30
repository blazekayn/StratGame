using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Global_Functions
/// </summary>
public class Global_Functions
{
    public Global_Functions()
    {
        //
        // TODO: Add constructor logic here
        //

    }
         /// <summary>
        /// Calculates Build Time for a building
        /// </summary>
        /// <param name="CurrentBuildingLevel"></param>
        /// <param name="BaseBuildTime"></param>
        /// <returns></returns>
        public Double GetBuildTime(int CurrentBuildingLevel, int BaseBuildTime)
    {
        Double Time = (Math.Pow(.005, BaseBuildTime) + CurrentBuildingLevel) * (CurrentBuildingLevel * Math.Pow(1.35, CurrentBuildingLevel));

        return Time;
    }

}