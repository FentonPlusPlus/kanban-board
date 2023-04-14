# Test _2

### 2_GivenARequestToRetrieveAListOfUsers

# ThenTheUserListIsReturnAlphabetically

```csharp
[Test]
        public void ThenTheUserListIsReturnedAlphabetically()
        {
            Assert.That(_usersResponse.Users[0].Username, Is.EqualTo("Anniyah French"));
            Assert.That(_usersResponse.Users[1].Username, Is.EqualTo("Arwel Kaye"));
            Assert.That(_usersResponse.Users[2].Username, Is.EqualTo("Brogan Vinson"));
            Assert.That(_usersResponse.Users[3].Username, Is.EqualTo("Dustin Schaefer"));
            Assert.That(_usersResponse.Users[4].Username, Is.EqualTo("Irving Weston"));
            Assert.That(_usersResponse.Users[5].Username, Is.EqualTo("Karla Ellis"));
            Assert.That(_usersResponse.Users[6].Username, Is.EqualTo("Kieran Edge"));
            Assert.That(_usersResponse.Users[7].Username, Is.EqualTo("Shayna Ortega"));
            Assert.That(_usersResponse.Users[8].Username, Is.EqualTo("Zaid Thorne"));
        }

```

### **What do we know?**

- We have a list of users and we are expecting that we can get them back in alphabetical order, and that we can get the appropriate initials for each name
- We are using the UsersController class to get all the users and store that in a private instance variable _usersResponse

```csharp
public class AllUsersResponse
    {
        public List<UserModel> Users { get; set; }
    }
```

- Users are stored in an array and accessed through this as _usersResponse.Users
- We can call on there Username and we can call on the Initials with .Username and .Initials

### The Problem

- When accessing the users alphabetically we get Karla Ellis before Irving Weston instead of the other way around

### **What is wrong?**

If we follow the data trail…

```csharp
public AllUsersResponse GetAll()
        {
            return _userService.GetAllUsers();
        }
// users controller calls on UserService

public AllUsersResponse GetAllUsers()
        {
            var userRecords = _userRepository.GetAll();
            var response = new AllUsersResponse {Users = new List<UserModel>()};

            foreach (var userRecord in userRecords)
            {
                response.Users.Add(new UserModel
                {
                    Username = userRecord.Username
                });
            }

            return response;
        }
// we are getting a list of users from the database in _userRepository and storing it in userRecords
// we're defining a response class in which we store all our userRecords into it's Users List property

//If we follow the UserRepository we can see
public List<UserRecord> GetAll()
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                var users =connection.Query<UserRecord>("SELECT * from user;");
                return users.ToList();
            }
        }
// The SQL query does not return the user table in alphabetical order. As the test uses this specific function and we cannot change the test, we must therefore edit the GetAll() function to return in alaphabetical order
```

### **********************How to fix?**********************

By changing the SQL query to take in a order by alphabetical clause

```csharp
public List<UserRecord> GetAll()
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                var users =connection.Query<UserRecord>("SELECT * from user ORDER BY username ASC;");
                return users.ToList();
            }
        }
```

### Notes

- I would personally create a seperate function for this so that we can distinguish by database order and alphabetical order

# ThenTheInitialsAreGeneratedForEachUser

```csharp

        [Test]
        public void ThenTheInitialsAreGeneratedForEachUser()
        {
            Assert.That(_usersResponse.Users[0].Initials, Is.EqualTo("AF"));
        }
```

### **What do we know?**

- We have a list of users and we are expecting  that when call on the Initials property, we will recieve the correct initials

### The Problem

- When calling the initials of Annyiah French we get Null instead of AF

### **What is wrong?**

If we follow the data trail…

```csharp
public void Setup()
        {
            _testEngine = new TestEngine();
          
            _usersResponse = _testEngine.UsersController.GetAll();
        }
// usersResponse is an instance of AllUsersResponse
// which contains a list<UserModel> of Users

public class AllUsersResponse
    {
        public List<UserModel> Users { get; set; }
    }

//which in turn contains these methods
public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Initials { get; set; }
    }

// as the data from the SQL script does not store a Initials column, we therefore are retrieving null as there is no way to access this data
```

### **********************How to fix?**********************

By modifying the get Initials method to get the Username and then return only the initials

```csharp
public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Initials
        {
            /// splits the username based on empty spaces and returns the first character, then joins them together to retrieve the initials
            get
            {
                if (string.IsNullOrWhiteSpace(Username))
                {
                    return string.Empty;
                }
                return string.Join("", Username.Split(' ')
                    .Select(name => name.Substring(0, 1)));
            }
        }

        public string GetInitials()
        {
            return Initials;
        }
    }
```