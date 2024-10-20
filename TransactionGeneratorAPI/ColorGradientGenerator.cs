namespace TransactionGeneratorAPI
{
    public class ColorGradientGenerator
    {
        public static string[] GenerateHexGradient(int steps)
        {
            // Define the start and end colors
            string startColor = "#008000";
            string endColor = "#eeffee";

            // Convert hex to RGB
            int startRed = int.Parse(startColor.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            int startGreen = int.Parse(startColor.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            int startBlue = int.Parse(startColor.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

            int endRed = int.Parse(endColor.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            int endGreen = int.Parse(endColor.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            int endBlue = int.Parse(endColor.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

            // Calculate step sizes
            int stepRed = (endRed - startRed) / steps;
            int stepGreen = (endGreen - startGreen) / steps;
            int stepBlue = (endBlue - startBlue) / steps;

            // Generate gradient
            List<string> gradient = new List<string>(steps);
            for (int i = 0; i <= steps; i++)
            {
                int red = Math.Min(Math.Max(startRed + (stepRed * i), 0), 255);
                int green = Math.Min(Math.Max(startGreen + (stepGreen * i), 0), 255);
                int blue = Math.Min(Math.Max(startBlue + (stepBlue * i), 0), 255);

                gradient.Add($"#{red:X2}{green:X2}{blue:X2}");
            }

            return gradient.ToArray();
        }
    }
}
