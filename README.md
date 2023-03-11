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
![LimitWords](https://github.com/samasargent/JobPlacementDashboard/blob/main/limitwordsexample.png)

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

The next model I created was CastDirector, which was a user who was an admin in the Production area for the CastMember model. It extended from ApplicationUser.
To test out the class, I created a seed method for CastDirector, which included setting up a role called Cast Director.

```c#
public static void SeedCastDirector(ApplicationDbContext context)
    {
        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

        if (!roleManager.RoleExists("CastDirector"))
        {
            var role = new IdentityRole();
            role.Name = "CastDirector";
            roleManager.Create(role);
        }

        var director = new CastDirector
        {
            UserName = "namename",
            Email = "namename@gmail.com",
            EmailConfirmed = true,
            PhoneNumber = "555-555-5555",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0,
            HiredCastMembers = 20,
            FiredCastMembers = 5
        };

        var chkUser = UserManager.Create(director);

        if (chkUser.Succeeded)
        {
            var result = UserManager.AddToRole(director.Id, "CastDirector");
        }
    }
```

By setting up the role CastDirector, I was then able to restrict access to the Create, Edit and Delete pages so only a user assigned that role could make changes to those. If a general user tried to access these pages, they would be redirected to the Access Denied page.

```c#
public class CastDirectorAuthorize : AuthorizeAttribute
{
    // Called when access is denied
    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(
        new RouteValueDictionary(new { controller = "CastMembers", action = "AccessDenied" }));
    }
}
```

_Jump to: [Front End Stories](#front-end-stories), [Back End Stories](#back-end-stories), [Other Skills](#other-skills-learned), [Page Top](#live-project)_

## Front End Stories

* [Create and Edit Pages](#create-and-edit-pages)
* [Index Page](#index-page)
* [Details and Delete Pages](#details-and-delete-pages)
* [Access Denied Page](#access-denied-page)

### Create and Edit Pages

For the styling of the Credit and Edit pages, I initially had the following requirements:

* Style the Submit and Back to List buttons. There needed to be a color distinction between these two buttons and they both be needed to be centered on the page.
* Add placeholders to all input fields. 
* Change the border color of the input fields when clicked.
* Place the form in a centered container.

Later on, I also needed to add the image upload section, as well as the preview of that image.

![Create Page GIF](https://github.com/samasargent/JobPlacementDashboard/blob/main/createpage.gif)
 
### Index Page

For the styling and layout of the Index page, I needed to set the page up so each cast member loaded as a card and was sorted under the production they were acting in. Next, each card should have an overlay with edit and delete links appearing over the image on hover.

```html
@foreach (var prod in productions)
{
<div>
    <h3 class="mt-5">
        @Html.DisplayFor(modelItem => prod.Key)
    </h3>
    <hr class="bg-light" />
    <div class="row card-columns">
        @foreach (var member in Model.Where(m => m.ProductionTitle == prod.Key))
        {
            <div class="col-sml-4 p-1">
                <div class="card m-2 castMember-Index--card">
                    <div class="castMember-Index--overlaycontainer">
                        <div class="castMember-Index--overlaybuttons text-center mt-5">
                            <i class="fas fa-pencil fa-2x castMember-Index--btn" onclick="location.href='@Url.Action("Edit", "CastMembers", new { id = member.CastMemberID })'"></i>
                            <i class="fas fa-trash-alt fa-2x castMember-Index--btn" onclick="location.href='@Url.Action("Delete", "CastMembers", new { id = member.CastMemberID })'"></i>
                        </div>

                        <a href="@Url.Action("Details", "CastMembers", new { id = member.CastMemberID })">
                            <img src="@CastMembersController.ImageSource(member.Photo)" class="card-img-top castMember-Index--image" alt="@member.CastMemberID">
                        </a>

                        <div class="card-body bg-dark">
                            <h5 class="card-title text-center font-weight-bold">@member.Name </h5>
                        </div>
                    </div>
                </div>
            </div>
        }
    </div>
</div>
}
```
Later on, I added a search feature to the top of the page so that you could find a cast member, either by searching a name or part of a name or something in their biography.

![Index Page GIF](https://github.com/samasargent/JobPlacementDashboard/blob/main/indexpage.gif)

### Details and Delete Pages

### Access Denied Page

_Jump to: [Front End Stories](#front-end-stories), [Back End Stories](#back-end-stories), [Other Skills](#other-skills-learned), [Page Top](#live-project)_

## Other Skills Learned

* _How to break down a story into smaller parts._ More than once, I would have a story that would seem complicated a first. I figured out how to break it down into steps that I could do. Then I would have a series of steps to complete, rather than a nebulous goal.
* _How to research a new subject to solve a problem._ Many times on this project, I ran into problems I needed to solve that I had little to no familiarity with. I quickly learned how to find the kind of answers I needed through persisting in research until I understood the subject enough to come up with a solution for the story.
* _How utilize version control to better work as part of a team._ I became more familiar with using branches and making changes to code alongside other people. I learned how important it is to stay in communication with the people you're working with. Also I learned from experience how valuable making frequent commits and checking in code can prevent a lot of problems that would be caused by waiting to save or test code until you have a big chunk of it.

_Jump to: [Front End Stories](#front-end-stories), [Back End Stories](#back-end-stories), [Other Skills](#other-skills-learned), [Page Top](#live-project)_


