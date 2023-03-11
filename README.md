# Live Project

## Introduction

I worked as part of a small team to create an interactive website for a theatre company local to Portland. The purpose of the website was to manage content and productions for the company. The project waas built using ASP .NET MVC and Entity Framework. I mainly worked on the Cast Member section of the website, where data on the different members of the company would be stored. We worked in a two week sprint to get the basic structure of the website set up and designed. I worked on several [back end stories](#back-end-stories)—mostly setting up two of the main models for the website. I also worked on the [front end](#front-end-stories) of the website, utilizing Bootstrap and JavaScript to design the CRUD pages for the CastMember class. I learned good version control and [other skills](#other-skills-learned) from working on this team over the two weeks.

Below are descriptions of the stories I worked on, along with code snippets and navigation links. I also have some full code files in this repo for the larger functionalities I implemented.

## Back End Stories

* [Limit Words Method](#limit-words-method)
* [CastMember Model](#castmember-model)
* [CastDirector Model](#castdirector-model)

### [Limit Words Method](https://github.com/samasargent/JobPlacementDashboard/blob/main/LimitWords.cs)

My task was to create a method that could be used to limit the number of words that are displayed using ellipses. For example, "Lorem Ipsum Lorem Ipsum Lorem Ipsum" would turn into "Lorem Ipsum Lorem...". The method took in a string and an integer—the string was the content and the integer was how many words were allowed before cutting off the string and adding ellipses ( . . . ).

I worked it out into two parts. The first was determining how many words there were, ignoring any leading or trailing white spaces. The second part split up the string into words and limited the number of them to the integer specified.

As an example, I was able to use the method to limit the words that loaded from a cast member's biography.
```c#
@TextHelpers.LimitWords((@Model.Bio.ToString()), 10)
```
![myImage](https://github.com/samasargent/JobPlacementDashboard/blob/main/limitwordsexample.png)

### [CastMember Model](https://github.com/samasargent/JobPlacementDashboard/blob/main/CastMember.cs)

The first part was creating an entity model for the CastMember class. I created a model for CastMember and an enum for positions, and set up CRUD functionality.
The model included a property for storing uploaded photos as a byte array.

After setting up the CRUD pages for CastMember, I created a method in the CastMember Controller that had a parameter for an uploaded photo and converted that photo into a byte[]. This allowed users to upload a photo that would be saved in the database entry for that cast member. I wrote another method for retrieving the byte array for the image from the database and converting it to a base 64 string that could be used to display it.

```c#
[HttpPost]
public byte[] UploadPhoto(HttpPostedFileBase photoUpload)
{
    byte[] bytes;
    BinaryReader br = new BinaryReader(photoUpload.InputStream);
    bytes = br.ReadBytes(photoUpload.ContentLength);
    return bytes;
}

public static string ImageSource(byte[] photo)
{
    // If there is a byte array stored at CastMember.Photo
    if (photo != null && photo.Length > 0)
    {
        // Gets the mime type of the image file
        string imageType = GetImageMimeType(photo);
        // Converts the byte array to a base 64 
        string base64 = Convert.ToBase64String(photo);
        // Returns the mime type and byte array of the image file written as below
        return string.Format("data:{0};base64,{1}", imageType, base64);
    }
    // Else, return null
    else
    {
        return null;
    }
}
```

### CastDirector Model

_Jump to: [Front End Stories](#front-end-stories), [Back End Stories](#back-end-stories), [Other Skills](#other-skills-learned), [Page Top](#live-project)_

## Front End Stories

* [Create and Edit Pages](#create-and-edit-pages)
* [Index Page](#index-page)
* [Details and Delete Pages](#details-and-delete-pages)
* [Access Denied Page](#access-denied-page)

### Create and Edit Pages

### Index Page

### Details and Delete Pages

### Access Denied Page

_Jump to: [Front End Stories](#front-end-stories), [Back End Stories](#back-end-stories), [Other Skills](#other-skills-learned), [Page Top](#live-project)_

## Other Skills Learned

* _How to break down a story into smaller parts._ More than once, I would have a story that would seem complicated a first. I figured out how to break it down into steps that I could do. Then I would have a series of steps to complete, rather than a nebulous goal.
* _How to research a new subject to solve a problem._ Many times on this project, I ran into problems I needed to solve that I had little to no familiarity with. I quickly learned how to find the kind of answers I needed through persisting in research until I understood the subject enough to come up with a solution for the story.
* _How utilize version control to better work as part of a team._ I became more familiar with using branches and making changes to code alongside other people. I learned how important it is to stay in communication with the people you're working with. Also I learned from experience how valuable making frequent commits and checking in code can prevent a lot of problems that would be caused by waiting to save or test code until you have a big chunk of it.

_Jump to: [Front End Stories](#front-end-stories), [Back End Stories](#back-end-stories), [Other Skills](#other-skills-learned), [Page Top](#live-project)_


