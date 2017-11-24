using UnityEngine.UI;
using UnityEditor;
//using Unity
/// Zambari 2017


public class zEditorTestEditorWindow : zEditorTemplate
{    
	
	[MenuItem("Tools/Open zEditorTestEditorWindow ")]
   static void Init()
   {
        BaseInit(typeof(zEditorTestEditorWindow));
   }
	
	protected override string baseName { get { return "Template"; } set { } }  // override
    protected override bool showConfig { get { return false; } set { } }

	  protected override void AddTabs()
    {
        AddTab("myTab");
        AddTab("ifSomeImagesSelected");
        AddTab("ifAllImages");
    }
    protected override bool ShouldTabBeVisible(string s) // override to enable context sensitive tab switching
    {
        if (s == "ifSomeImagesSelected") return SelectionHasComponentsSome<Image>();
        if (s == "ifAllImages") return SelectionHasComponentsAll<Image>();
        return true;
    }
    protected override void DisplayTab(string tabName)
    {
        if (tabName == "myTab") DisplayMyTab1();
        if (tabName == "ifSomeImagesSelected") DisplayWhenSomeImages();
        if (tabName == "ifAllImages") DisplayWhenAllImages();
    }

 
    void DisplayMyTab1()
    {
        Label(" always on");
    }
    void DisplayWhenSomeImages()
    {
        Label(" this tabis onl visible when any objects have image");
    }
    void DisplayWhenAllImages()
    {
        Label(" this tabis onl visible when all objects have image");
    }




}
