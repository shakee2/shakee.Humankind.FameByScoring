using UnityEngine;

namespace shakee.Humankind.FameByScoring
{
    public static partial class Styles
    {
        public static Color GreenTextColor { get; set; } = (Color) new Color32(80, 230, 80, 220);
        public static Color WhiteTextColor { get; set; } = (Color) new Color32(250, 250, 250, 255);
        public static Color WhitePearlTextColor { get; set; } = (Color) new Color32(200, 200, 200, 255);
        public static Color DarkTextColor { get; set; } = (Color) new Color32(10, 10, 10, 190);
        public static Color DarkishTextColor { get; set; } = (Color) new Color32(10, 10, 10, 150);
        public static Color BlueTextColor { get; set; } = (Color) new Color32(40, 86, 240, 255);
        public static Color GoldTextColor { get; set; } = new Color(0.85f, 0.75f, 0f, 0.85f);

        public static GUIStyle Alpha50BlackBackgroundStyle { get; set; } = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                //background = Alpha50BlackPixel,
                //textColor = Color.white
            }
        };
    }

    public static class BackgroundStyle
    {
        public static Color GreenTextColor { get; set; } = (Color) new Color32(80, 230, 80, 220);
        public static Color WhiteTextColor { get; set; } = (Color) new Color32(250, 250, 250, 255);
        public static Color WhitePearlTextColor { get; set; } = (Color) new Color32(200, 200, 200, 255);
        public static Color DarkTextColor { get; set; } = (Color) new Color32(10, 10, 10, 190);
        public static Color DarkishTextColor { get; set; } = (Color) new Color32(10, 10, 10, 150);
        public static Color BlueTextColor { get; set; } = (Color) new Color32(40, 86, 240, 255);
        public static Color GoldTextColor { get; set; } = new Color(0.85f, 0.75f, 0f, 0.85f);
        public static Color SilverTextColor { get; set; } = new Color32(192, 192, 192, 255);
        public static Color BronzeTextColor { get; set; } = new Color32(205, 127, 50, 255);
        public static Color OtherTextColor { get; set; } = new Color32(169, 169, 169, 255);
       // private static GUIStyle style = new GUIStyle();
        private static Texture2D texture = new Texture2D(1, 1);
    
    
        public static GUIStyle Get(Color color)
        {
            GUIStyle style = new GUIStyle();
            texture.SetPixel(0, 0, color);
            texture.Apply();
            style.normal.background = texture;
            return style;
        }

        public static GUIStyle Headline(Color color)
        {
            GUIStyle style = new GUIStyle();
            texture.SetPixel(0, 0, color);
            texture.Apply();
            style.fontSize = 24;
            style.alignment = TextAnchor.MiddleCenter;
            style.padding = new RectOffset(5,5,5,5);            
            //style.font = new Font("Arial");
            style.normal.background = texture;
            style.normal.textColor = Color.white;
            return style;
        }
        public static GUIStyle NormalCenter(int fontsize)
        {
            GUIStyle style = new GUIStyle();
            TextAnchor blub = new TextAnchor{};
            //texture.SetPixel(0, 0, color);
            //texture.Apply();
            style.fontSize = fontsize;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;    
            style.padding = new RectOffset(6,6,6,6);   
            //style.margin = new RectOffset(5,5,5,5);
            //style.normal.background = texture;       
            return style;
        }
        public static GUIStyle NormalLeft(int fontsize)
        {
            GUIStyle style = new GUIStyle();
            TextAnchor blub = new TextAnchor{};
            //texture.SetPixel(0, 0, color);
            //texture.Apply();
            style.fontSize = fontsize;
            style.alignment = TextAnchor.MiddleLeft;
            style.normal.textColor = Color.white;
            style.padding = new RectOffset(6,6,6,6);  
            
            //style.margin = new RectOffset(5,5,5,5);   
            //style.normal.background = texture;       
            return style;
        }
        public static GUIStyle RankColor(string rank)
        {
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            
            style.padding = new RectOffset(6,6,6,6);
            
            switch (rank)            
            {
                
                case "1st":
                    style.normal.textColor = BackgroundStyle.GoldTextColor;
                    style.fontSize = 13;
                    return style;
                case "2nd":
                    style.normal.textColor = BackgroundStyle.SilverTextColor;
                    style.fontSize = 13;
                    return style;
                case "3rd":
                    style.normal.textColor = BackgroundStyle.BronzeTextColor;
                    style.fontSize = 13;
                    return style;
                default:
                    style.normal.textColor = BackgroundStyle.OtherTextColor;
                    style.fontSize = 13;
                    return style;          
            }
        }
    }

    

}