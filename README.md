# TrustApplication

![moqup](https://user-images.githubusercontent.com/32786237/136834531-5aacb802-ea73-4603-851a-ec960d92ed4c.PNG)
<br/>
AS A business user <br/>
I WANT to upload files <br/>
AND view list of uploaded files when uploaded successfully <br/>
SO THAT I am able to provide files for later business BAU usage <br/>
<br/>
Project: Files Import for General BAU use <br/>

In Scope:

	0. On (new) Page load
		- Endpoint
			- default: <localhost, no defined endpoint>
		- Encryption
			- default: HTTP
		- Access:
			- default: Unrestricted
		- load (new) Upload Area component
			- Appearance
				- Rectangular box
					- Dimensions
						- default: width is % of parent element width, height is 1/4 of width
					- White background
					- Black text inside, centered vertically horizontally
						- Text shown
							- default: "Browse for files" CrLf "(or drag and drop files)"
							- non-default: 
								- Phone: "Browse for files" CrLf "(press here)"
		- load (new) Display Uploaded grid component
			- Appearance
				- Table
				- Row width
					default: width is % of parent element width
				- Row height
					default: padding all sides: pixels
				- Header row(s) Grey: HEX #E5E5E5
					- On click Header cell action
						- nothing happens
					- default: 2 columns: "Filename" & "Uploaded Date"
				- Non-header rows
					Minimum
						- default: 1 row, empty cells
						- non-default: <props Modifiable integer>
					Maximum
						- default: none
					- Scrolling
						- default: none
				- Standards/Consistency
					- Filename
						- Only display <filename>.<extension>
						- Max length displayable within cell for filename
							- Trim filename (excl. ".extension") to be within (filename + extension)'s max length
					- Uploaded Date
						- Format (incl. of Date + Time)
							- Stored as
								- default: UTC
							- Display to User
								- default: "dd/MM/yyyy HH:mm" //19/04/2021 16:41
					- Sorting
						- default: ASCENDING by Uploaded Date
				- Interactability
					- nothing happens
			- Length of time to display
				- default:
					- Append on successful 
					- Loses all data on page refresh
	1. On User interacting with component
		- Click
		- Drag and drop files
	2. On User locating file(s)
		- Opens 'Browse for file(s)' window provided by browser
		- Ability to select 1 or more file(s)
		- Click 'Cancel' or 'X' button to Abort
			- Revert back to state 0.
		- Click 'Select' to proceed
			- Continue to state 3.
		- Validation
			- *refer to section below "Input restrictions & validations (on all file(s))"
	3. Time between Uploading File(s) and Uploaded successfully
		- Front-end
			- UI Appearance change
				- Text
					- "Uploading…”
			- UI Interactability
				- none
				- No explicit capability to abort uploading apart from closing tab
			- Request
				Endpoint: see Back-end endpoint
				Content-Type: application/json
				Body: Filename, File’s Extension, File’s content
			- Back-end
				- Endpoint: localhost/api/Customer/PostFileUpload
				- Validation
					- *refer to section below "Input restrictions & validations (on all file(s))"
				- Storage:
					- In-memory database within 
	4. Response from API arrival
		- API sends back as a response
			- Success (2XX)
				- File data successfully uploaded
		- [ { filename: <Filename>, dateTimeUploaded: <Uploaded Date> }, ... ]
			- Unsuccessful (4XX or 5XX)
				- TBC
		- Revert 'Upload Area component' to state 0.
		- Append response file data onto existing "Display Uploaded grid component"

	
	Input restrictions & validations (on all file(s))
		- On each file:
		    - File is a valid type to be saved into Database
		    - Filename should not exceed 255 characters //Windows’ limit on filenames


	Technicality
		- (new) URL Page
		- (new) React Components
		  - Responsive
		  - Reusability
			- Mobile app reusable (e.g. React Native)
			- Web app reusable
		- (new) .NET Core Web API Solution, architecture involved
		- (new) Consumable by .NET Core API
		  - Able to use in-memory database
		- Vertical slice
		  - Feasibility: Start off with attempting to import into API server in-memory and return file metadata before UI
		- Source code available on GitHub
		- Time required to research and learn on unfamiliar areas

	Testing
		- Manual testing
		
					
Out of Scope:

		- Upload Folder(s)
		- Not-200 status code paths (flow for Validation errors or Exceptions)
		- Separation of Files’ Extension from its Filename
		- Restrictions to File(s) allowed to be uploaded (see Validations section)
			- Quantity of File(s) restriction
			- Filename length restriction
			- File extension type, length restriction
			- Filesize restriction
			- File content restriction
		- Anti-virus checks on file(s)
		- Malicious request(s)
			- Restricting quantity of file(s) or bandwidth limit on ip address/user
		- Duplication of Files
		- Masking filename to fetch from lookup table
		- Persistent storage of user's uploaded files
		- User able to download their Uploaded file(s)
		- Preview of Uploaded file(s)
		- Notify user of any file(s) unsuccessfully uploaded
		- Testing: 
				- Unit test (+ Moq)
				- End to End
				- Selenium WebDriver
