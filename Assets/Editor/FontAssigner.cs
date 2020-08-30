using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

public class FontAssigner : ScriptableWizard
{
    public Font targetFont;
    public Font replacementFont;
    public bool replaceAllFonts;

    [MenuItem("Tools/Assign Font")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<FontAssigner>("Swap Fonts", "Apply");
        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
    }

    void OnWizardCreate()
    {
        Text[] text = Resources.FindObjectsOfTypeAll<Text>();
        TextMesh[] textMesh = Resources.FindObjectsOfTypeAll<TextMesh>();
        for(int i = 0; i < text.Length; i++)
        {
            if(text[i].font == targetFont || replaceAllFonts)
            {
                text[i].font = replacementFont;
            }
        }

        for (int i = 0; i < textMesh.Length; i++)
        {
            if (textMesh[i].font == targetFont || replaceAllFonts)
            {
                textMesh[i].font = replacementFont;
            }
        }

        text = Resources.FindObjectsOfTypeAll<Text>();
        textMesh = Resources.FindObjectsOfTypeAll<TextMesh>();
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i].font == targetFont || replaceAllFonts)
            {
                text[i].font = replacementFont;
            }
        }

        for (int i = 0; i < textMesh.Length; i++)
        {
            if (textMesh[i].font == targetFont || replaceAllFonts)
            {
                textMesh[i].font = replacementFont;
            }
        }
    }

    void OnWizardUpdate()
    {
        helpString = "Set the font you would like to change out and the font to change it to";
    }
}
