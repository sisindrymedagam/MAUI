using System.Collections.ObjectModel;

namespace Passkeeper.Models;

public class PasswordListView
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Company { get; set; } //this can be repeated 

    public string? CompanyIcon { get; set; } //same as Company

    public string? Url { get; set; }
}

public class CompanyListView
{
    public string? Company { get; set; } //this can be repeated 

    public string? CompanyIcon { get; set; } //same as Company

    public int AccountsCount { get; set; }

    public string AccountsDisplay =>
    AccountsCount > 1 ? $"{AccountsCount} credentials" : $"{AccountsCount} credential";
}

public class Password
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? PasswordEncrypted { get; set; }

    public string? Company { get; set; } //this can be repeated 

    public string? CompanyIcon { get; set; } //same as Company

    public string? Category { get; set; } //this can be repeated 

    public string? CategoryIcon { get; set; } //same as Group

    public string? Url { get; set; }

    public string? Note { get; set; }
}

public class PasswordsList
{
    public static ObservableCollection<PasswordListView> GetPasswordListView()
    {
        IEnumerable<PasswordListView> passwordList = BaseData().Select(d => new PasswordListView
        {
            Id = d.Id,
            Company = d.Company,
            CompanyIcon = d.CompanyIcon,
            Email = d.Email,
            Url = d.Url,
        });

        return new ObservableCollection<PasswordListView>(passwordList); ;
    }

    public static ObservableCollection<CompanyListView> GetCompanyListView()
    {
        IEnumerable<CompanyListView> companyList = BaseData().GroupBy(s => new { s.Company, s.CompanyIcon }).Select(d => new CompanyListView
        {
            Company = d.Key.Company,
            CompanyIcon = d.Key.CompanyIcon,
            AccountsCount = d.Count()
        });

        return new ObservableCollection<CompanyListView>(companyList); ;
    }

    private static List<Password> BaseData()
    {
        List<Password> passwordList =
            [
                new Password { Id=1, Email="user@facebook.com", PasswordEncrypted="FbEnc1!", Company="Facebook", CompanyIcon="https://img.icons8.com/color/48/000000/facebook-new.png", Category="Social", CategoryIcon="https://img.icons8.com/fluency/48/000000/group-foreground-selected.png", Url="https://facebook.com", Note="Facebook login" },
                new Password { Id=2, Email="user@twitter.com", PasswordEncrypted="TwEnc2@", Company="Twitter", CompanyIcon="https://img.icons8.com/color/48/000000/twitter-squared.png", Category="Social", CategoryIcon="https://img.icons8.com/fluency/48/000000/group-foreground-selected.png", Url="https://twitter.com", Note="Twitter login" },
                new Password { Id=3, Email="user@linkedin.com", PasswordEncrypted="LnEnc3#", Company="LinkedIn", CompanyIcon="https://img.icons8.com/color/48/000000/linkedin.png", Category="Social", CategoryIcon="https://img.icons8.com/fluency/48/000000/group-foreground-selected.png", Url="https://linkedin.com", Note="LinkedIn login" },

                new Password { Id=4, Email="user@google.com", PasswordEncrypted="GgEnc4$", Company="Google", CompanyIcon="https://img.icons8.com/color/48/000000/google-logo.png", Category="Work", CategoryIcon="https://img.icons8.com/fluency/48/000000/briefcase.png", Url="https://workspace.google.com", Note="Google Workspace" },
                new Password { Id=5, Email="user@gmail.com", PasswordEncrypted="GmailEnc5%", Company="Google", CompanyIcon="https://img.icons8.com/color/48/000000/google-logo.png", Category="Email", CategoryIcon="https://img.icons8.com/fluency/48/000000/email.png", Url="https://gmail.com", Note="Gmail" },
                new Password { Id=6, Email="user@youtube.com", PasswordEncrypted="YtEnc6^", Company="Google", CompanyIcon="https://img.icons8.com/color/48/000000/google-logo.png", Category="Entertainment", CategoryIcon="https://img.icons8.com/fluency/48/000000/movie-projector.png", Url="https://youtube.com", Note="YouTube" },

                new Password { Id=7, Email="user@apple.com", PasswordEncrypted="ApEnc7&", Company="Apple", CompanyIcon="https://img.icons8.com/ios-filled/50/000000/mac-client.png", Category="Tech", CategoryIcon="https://img.icons8.com/fluency/48/000000/laptop.png", Url="https://appleid.apple.com", Note="Apple ID" },
                new Password { Id=8, Email="user@icloud.com", PasswordEncrypted="iEnc8*", Company="Apple", CompanyIcon="https://img.icons8.com/ios-filled/50/000000/mac-client.png", Category="Cloud", CategoryIcon="https://img.icons8.com/fluency/48/000000/cloud.png", Url="https://icloud.com", Note="iCloud" },

                new Password { Id=9, Email="user@microsoft.com", PasswordEncrypted="MsEnc9(", Company="Microsoft", CompanyIcon="https://img.icons8.com/fluency/48/000000/microsoft.png", Category="Work", CategoryIcon="https://img.icons8.com/fluency/48/000000/briefcase.png", Url="https://microsoft.com", Note="Microsoft login" },
                new Password { Id=10, Email="user@outlook.com", PasswordEncrypted="OutEnc10)", Company="Microsoft", CompanyIcon="https://img.icons8.com/fluency/48/000000/microsoft.png", Category="Email", CategoryIcon="https://img.icons8.com/fluency/48/000000/email.png", Url="https://outlook.com", Note="Outlook" },

                new Password { Id=11, Email="user@amazon.com", PasswordEncrypted="AmaEnc11!", Company="Amazon", CompanyIcon="https://img.icons8.com/color/48/000000/amazon.png", Category="Shopping", CategoryIcon="https://img.icons8.com/fluency/48/000000/shopping-cart.png", Url="https://amazon.com", Note="Amazon" },
                new Password { Id=12, Email="user@primevideo.com", PasswordEncrypted="PvEnc12@", Company="Amazon", CompanyIcon="https://img.icons8.com/color/48/000000/amazon.png", Category="Entertainment", CategoryIcon="https://img.icons8.com/fluency/48/000000/movie-projector.png", Url="https://primevideo.com", Note="Prime Video" },

                new Password { Id=13, Email="user@netflix.com", PasswordEncrypted="NetEnc13#", Company="Netflix", CompanyIcon="https://img.icons8.com/color/48/000000/netflix.png", Category="Entertainment", CategoryIcon="https://img.icons8.com/fluency/48/000000/movie-projector.png", Url="https://netflix.com", Note="Netflix login" },
                new Password { Id=14, Email="user@spotify.com", PasswordEncrypted="SpoEnc14$", Company="Spotify", CompanyIcon="https://img.icons8.com/color/48/000000/spotify.png", Category="Entertainment", CategoryIcon="https://img.icons8.com/fluency/48/000000/movie-projector.png", Url="https://spotify.com", Note="Spotify" },

                new Password { Id=15, Email="user@paypal.com", PasswordEncrypted="PayEnc15%", Company="PayPal", CompanyIcon="https://img.icons8.com/color/48/000000/paypal.png", Category="Banking", CategoryIcon="https://img.icons8.com/fluency/48/000000/bank.png", Url="https://paypal.com", Note="PayPal" },
                new Password { Id=16, Email="user@chase.com", PasswordEncrypted="ChEnc16^", Company="Chase", CompanyIcon="https://img.icons8.com/color/48/000000/chase-bank.png", Category="Banking", CategoryIcon="https://img.icons8.com/fluency/48/000000/bank.png", Url="https://chase.com", Note="Bank login" },

                new Password { Id=17, Email="user@airbnb.com", PasswordEncrypted="AirEnc17&", Company="Airbnb", CompanyIcon="https://img.icons8.com/color/48/000000/airbnb.png", Category="Travel", CategoryIcon="https://img.icons8.com/fluency/48/000000/travel.png", Url="https://airbnb.com", Note="Airbnb login" },
                new Password { Id=18, Email="user@booking.com", PasswordEncrypted="BooEnc18*", Company="Booking", CompanyIcon="https://img.icons8.com/fluency/48/000000/booking.png", Category="Travel", CategoryIcon="https://img.icons8.com/fluency/48/000000/travel.png", Url="https://booking.com", Note="Booking.com" },

                new Password { Id=19, Email="user@uber.com", PasswordEncrypted="UbeEnc19(", Company="Uber", CompanyIcon="https://img.icons8.com/ios-filled/50/000000/uber.png", Category="Transport", CategoryIcon="https://img.icons8.com/fluency/48/000000/car.png", Url="https://uber.com", Note="Uber" },
                new Password { Id=20, Email="user@lyft.com", PasswordEncrypted="LyfEnc20)", Company="Lyft", CompanyIcon="https://img.icons8.com/fluency/48/000000/lyft.png", Category="Transport", CategoryIcon="https://img.icons8.com/fluency/48/000000/car.png", Url="https://lyft.com", Note="Lyft" },

                new Password { Id=21, Email="user@notion.so", PasswordEncrypted="NotEnc21!", Company="Notion", CompanyIcon="https://img.icons8.com/fluency/48/000000/notion.png", Category="Productivity", CategoryIcon="https://img.icons8.com/fluency/48/000000/task.png", Url="https://notion.so", Note="Notion workspace" },
                new Password { Id=22, Email="user@trello.com", PasswordEncrypted="TreEnc22@", Company="Trello", CompanyIcon="https://img.icons8.com/color/48/000000/trello.png", Category="Productivity", CategoryIcon="https://img.icons8.com/fluency/48/000000/task.png", Url="https://trello.com", Note="Trello boards" },

                new Password { Id=23, Email="user@dropbox.com", PasswordEncrypted="DrEnc23#", Company="Dropbox", CompanyIcon="https://img.icons8.com/fluency/48/000000/dropbox.png", Category="Cloud", CategoryIcon="https://img.icons8.com/fluency/48/000000/cloud.png", Url="https://dropbox.com", Note="Dropbox" },
                new Password { Id=24, Email="user@drive.com", PasswordEncrypted="DrvEnc24$", Company="Google", CompanyIcon="https://img.icons8.com/color/48/000000/google-logo.png", Category="Cloud", CategoryIcon="https://img.icons8.com/fluency/48/000000/cloud.png", Url="https://drive.google.com", Note="Google Drive" },

                new Password { Id=25, Email="user@coursera.org", PasswordEncrypted="CouEnc25%", Company="Coursera", CompanyIcon="https://img.icons8.com/color/48/000000/coursera.png", Category="Education", CategoryIcon="https://img.icons8.com/fluency/48/000000/graduation-cap.png", Url="https://coursera.org", Note="Online courses" },
                new Password { Id=26, Email="user@udemy.com", PasswordEncrypted="UdeEnc26^", Company="Udemy", CompanyIcon="https://img.icons8.com/color/48/000000/udemy.png", Category="Education", CategoryIcon="https://img.icons8.com/fluency/48/000000/graduation-cap.png", Url="https://udemy.com", Note="Udemy login" },

                new Password { Id=27, Email="user@reddit.com", PasswordEncrypted="RedEnc27&", Company="Reddit", CompanyIcon="https://img.icons8.com/color/48/000000/reddit.png", Category="Social", CategoryIcon="https://img.icons8.com/fluency/48/000000/group-foreground-selected.png", Url="https://reddit.com", Note="Reddit account" },
                new Password { Id=28, Email="user@slack.com", PasswordEncrypted="SlkEnc28*", Company="Slack", CompanyIcon="https://img.icons8.com/color/48/000000/slack-new.png", Category="Work", CategoryIcon="https://img.icons8.com/fluency/48/000000/briefcase.png", Url="https://slack.com", Note="Slack workspace" },

                new Password { Id=29, Email="user@zoom.us", PasswordEncrypted="ZooEnc29(", Company="Zoom", CompanyIcon="https://img.icons8.com/color/48/000000/zoom.png", Category="Work", CategoryIcon="https://img.icons8.com/fluency/48/000000/briefcase.png", Url="https://zoom.us", Note="Zoom meeting" },
                new Password { Id=30, Email="user@discord.com", PasswordEncrypted="DisEnc30)", Company="Discord", CompanyIcon="https://img.icons8.com/color/48/000000/discord-logo.png", Category="Social", CategoryIcon="https://img.icons8.com/fluency/48/000000/group-foreground-selected.png", Url="https://discord.com", Note="Discord" }
            ];
        return passwordList;
    }
}
