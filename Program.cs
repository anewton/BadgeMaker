using Avalonia;
using McMaster.Extensions.CommandLineUtils;

namespace BadgeMaker;

partial class Program
{
    static void Main(string[] args)
    {
        AppBuilder.Configure<Application>()
            .UsePlatformDetect()
            .SetupWithoutStarting();

        var app = new CommandLineApplication();
        app.HelpOption();
        app.Name = "BadgeMaker";
        app.Description = "A command line utility to generate status badges.";

        CommandOption leftOption = app.Option("-l|--left <LEFT_TEXT>", "Text for the left side of the badge.\r\n", CommandOptionType.SingleValue);

        CommandOption rightOption = app.Option("-r|--right <RIGHT_TEXT>", "Text for the right side of the badge.\r\n", CommandOptionType.SingleValue);

        CommandOption outputPathOption = app.Option("-o|--output <OUTPUT_PATH>", "Full path for the output of the badge svg file.\r\n", CommandOptionType.SingleValue);

        CommandOption rightColorOption = app.Option("-c|--color <COLOR>", "Hex Color for the right side of the badge.\r\n\r\nNamed colors:\r\n\tblue: '#007ec6' (Default)\r\n\tbrightgreen: '#4c1'\r\n\tgreen: '#97ca00'\r\n\tyellow: '#dfb317'\r\n\tyellowgreen: '#a4a61d'\r\n\torange: '#fe7d37'\r\n\tred: '#e05d44'\r\n\tgrey: '#555'\r\n\tlightgrey: '#9f9f9f'", CommandOptionType.SingleValue);

        app.OnExecute(() =>
        {
            string leftValue = leftOption.Value();
            string left = string.IsNullOrWhiteSpace(leftValue) ? "hi" : leftValue;

            string rightValue = rightOption.Value();
            string right = string.IsNullOrWhiteSpace(rightValue) ? "there" : rightValue;

            string rightColorValue = rightColorOption.Value();
            string rightColor = string.IsNullOrWhiteSpace(rightColorValue) ? "#007ec6" : rightColorValue;

            string outputPathValue = outputPathOption.Value();
            string output = string.IsNullOrWhiteSpace(outputPathValue) ? System.IO.Path.Combine(AppContext.BaseDirectory, "badge.svg") : outputPathValue;

            BadgeInfo badgeInfo = new(left, right, rightColor);
            File.WriteAllText(new System.IO.FileInfo(output).FullName, badgeInfo.GetBadgeSvgString());

            Console.WriteLine($"{output}");
        });

        app.Execute(args);
    }
}
