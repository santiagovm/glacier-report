using System;

namespace Glacier.Tools.CLI
{
    public static class StringExtensions
    {
        public static string FixedWidth(this string value, int totalWidth)
        {
            if (value.Length <= totalWidth)
            {
                return value.PadRight(totalWidth);
            }

            int middle = value.Length / 2;

            string leftSide = value.Substring(startIndex: 0, length: middle);

            string rightSide = value.Substring(startIndex: middle);

            int sideSize = ( totalWidth - 4 ) / 2;

            string leftSideTrimmed = leftSide.Substring(startIndex: 0, length: sideSize);

            string rightSideTrimmed = rightSide.Remove(startIndex: 0, count: rightSide.Length - sideSize);

            return string.Format("{0}....{1}", leftSideTrimmed, rightSideTrimmed);
        }

        public static string ToFriendlyFileSize(this long sizeBytes)
        {
            if (sizeBytes < 1024)
            {
                return string.Format("{0:n0} B", sizeBytes);
            }

            double sizeKiloBytes = sizeBytes / 1024.0;

            if (sizeKiloBytes < 1024)
            {
                return string.Format("{0:n2} KB", sizeKiloBytes);
            }

            double sizeMegaBytes = sizeKiloBytes / 1024.0;

            if (sizeMegaBytes < 1024)
            {
                return string.Format("{0:n2} MB", sizeMegaBytes);
            }

            double sizeGigaBytes = sizeMegaBytes / 1024.0;

            if (sizeGigaBytes < 1024)
            {
                return string.Format("{0:n2} GB", sizeGigaBytes);
            }

            double sizeTeraBytes = sizeGigaBytes / 1024.0;

            return string.Format("{0:n2} TB", sizeTeraBytes);
        }

        public static string ToCost(this long sizeBytes)
        {
            double bytesToGigaBytesFactor = Math.Pow(1024, 3);
            
            double sizeGigaBytes = sizeBytes / bytesToGigaBytesFactor;

            double cost = sizeGigaBytes * 0.01; // $0.01 / GB

            return string.Format("${0:n0}", cost);
        }
    }
}
