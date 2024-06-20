using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class FavIconHandler : MonoBehaviour
{
    // An invisible object to store the list of favourite destinations.  
    public TMP_Dropdown favouritesListDropdown;
    
    public TMP_Dropdown destinations;
    public Sprite favouriteIconAdded;
    public Sprite favouriteIconRemoved;
    
    private Button imageButton;
    private Image favIcon;
    private List<string> favouritesLoadedFromFile;
    
    [Serializable]
    public class DropdownOptions
    {
        public List<string> options;
    }

    private void OnEnable()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
#endif
    }

    void Start()
    {
        favIcon = GetComponent<Image>();
        imageButton = GetComponent<Button>();
        
        if (imageButton != null)
        {
            imageButton.onClick.AddListener(OnImageClick);
        }
    }
    
    private void OnImageClick()
    {
        // The method is to recognize status of the particular option whether it's already in favourites or not,
        // and then switch it accordingly after the favourite icon has been tapped

        // Find the parent dropdown item
        Transform parentItem = transform.parent;

        // Get the sibling index of the parent item, which corresponds to the index in the dropdown options list
        int itemIndex = parentItem.GetSiblingIndex() - 1;

        // finding the option, which corresponding favourite icon has been tapped
        if (itemIndex >= 0 && itemIndex < destinations.options.Count)
        {
            string optionText = destinations.options[itemIndex].text;
            // checking whether favorite icon is representing an option of an added or non-added to favorite and switching at accordingly
            if (destinations.options[itemIndex].image != favouriteIconAdded)
            {
                destinations.options[itemIndex].image = favouriteIconAdded;
                favIcon.sprite = favouriteIconAdded;
                Debug.Log("Option: " + optionText + " added to favourites.");
                favouritesListDropdown.options.Add(destinations.options[itemIndex]);
            }
            else
            {
                destinations.options[itemIndex].image = favouriteIconRemoved;
                favIcon.sprite = favouriteIconRemoved;
                RemoveOptionContainingText(optionText);
                Debug.Log("Option: " + optionText + " added to favourites.");
            }
            SaveFavouritesOptionsToFile();
            SortDestinationListWithFavouritesOnTopOfTheList();
        }
        else
        {
            Debug.LogError("Invalid item index: " + itemIndex);
        }
    }
    
    public void SaveFavouritesOptionsToFile()
    {
        Debug.Log("SaveFavouritesOptionsToFile");
        // Create a DropdownOptions object
        DropdownOptions favouritesOptions = new DropdownOptions
        {
            options = new List<string>()
        };

        // Populate the favouritesOptions with the favouritesListDropdown options
        foreach (var option in favouritesListDropdown.options)
        {
            favouritesOptions.options.Add(option.text);
        }

        favouritesOptions.options.Sort();
        // Serialize the object to JSON
        string json = JsonUtility.ToJson(favouritesOptions);

        // Define the path to save the file
        string path = Path.Combine(Application.persistentDataPath, "favouritesOptions.json");

        // Write the JSON string to a file
        File.WriteAllText(path, json);

        Debug.Log("favouritesOptions saved to: " + path);
        Debug.Log("fav saved json: " +json);
    }

    public void LoadFavouritesOptionsFromFile()
    {
        Debug.Log("LoadFavouritesOptionsFromFile");
        // Clear the existing favourites dropdown options (an invisible object to temporary store the favourites list)
        favouritesListDropdown.options.Clear();

        // Path to the file with favourites data
        string path = Path.Combine(Application.persistentDataPath, "favouritesOptions.json");

        // Check if the file exists
        if (File.Exists(path))
        {
            Debug.Log("favourites file exists.");
            // Read the JSON string from the file
            string json = File.ReadAllText(path);

            // Deserialize the JSON string to a DropdownOptions object
            DropdownOptions favouritesOptions = JsonUtility.FromJson<DropdownOptions>(json);
            
            // Populate the favouritesListDropdown with the loaded options and adding an appropriate icon to it
            foreach (var option in favouritesOptions.options)
            {
                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(option, favouriteIconAdded);
                favouritesListDropdown.options.Add(optionData);
            }
            
            Debug.Log("fav json: " +json);
            Debug.Log("favouritesListDropdown.options.Count: " +favouritesListDropdown.options.Count);
        }
        else
        {
            Debug.Log("fav File not found at: " + path);
        }
        
        SortDestinationListWithFavouritesOnTopOfTheList();
    }
    
    public void ClearFavourites()
    {
        favouritesListDropdown.options.Clear();
        SaveFavouritesOptionsToFile();
        LoadFavouritesOptionsFromFile();
    }
    private void RemoveOptionContainingText(string text)
    {
        // Find the index of the option that contains the specified text
        int indexToRemove = -1;
        for (int i = 0; i < favouritesListDropdown.options.Count; i++)
        {
            if (favouritesListDropdown.options[i].text.Contains(text))
            {
                indexToRemove = i;
                break;
            }
        }
        // If an option is found, remove it
        if (indexToRemove != -1)
        {
            favouritesListDropdown.options.RemoveAt(indexToRemove);
            Debug.Log("Option containing text '" + text + "' removed.");
        }
        else
        {
            Debug.Log("No option containing text '" + text + "' found.");
        }
    }
    
    private void SortDestinationListWithFavouritesOnTopOfTheList()
    {
        // Sort the destinations list by the values in list of option[..].text
        destinations.options = destinations.options.OrderBy(option => option.text).ToList();
        
        // Find the option with text "Choose Location" after the sorting and move it to the top of the list
        TMP_Dropdown.OptionData chooseLocationOption = destinations.options.FirstOrDefault(option => option.text == "Choose Location");
        if (chooseLocationOption != null)
        {
            // Remove the option from its current position
            destinations.options.Remove(chooseLocationOption);
            destinations.options.Insert(0, chooseLocationOption);
        }
       
        // Removing the favourite options from the destinations list in order to add them again at the beginning of the list
        // Double time because of the way Unity handles such methods, running it twice makes it more reliable.
        // (One time doesn't always remove all the necessary items).
        RemoveDestinationsMarkedAsFavourites();
        RemoveDestinationsMarkedAsFavourites();
        //Adding the favourite options on the top of the destinations list
        for (int i = favouritesListDropdown.options.Count; i > 0; i--)
        {
            TMP_Dropdown.OptionData option = favouritesListDropdown.options[i-1];
            destinations.options.Insert(1, option);
            Debug.Log("fav option added to the destinations: " + option.text + ", " + option.image.name);
        }
        destinations.RefreshShownValue();
    }

    private void RemoveDestinationsMarkedAsFavourites()
    {
        for (int i = 0; i < destinations.options.Count; i++)
        {
            if (destinations.options[i] == null) Debug.Log("fav The option no. " + i + " is null.");
            else
            {
                if (favouritesListDropdown.options.Any(o => o.text.Equals(destinations.options[i].text,
                        StringComparison.OrdinalIgnoreCase))) 
                { 
                    Debug.Log("fav option removed from destinations: " + destinations.options[i].text);
                    destinations.options.RemoveAt(i);
                }
            }
        }
    }
}