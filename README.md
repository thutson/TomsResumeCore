# Tom's Resume Website
[![Build status](https://dev.azure.com/thutson79/TomsResumeCore/_apis/build/status/tomsresumeappserv%20-%20CI)](https://dev.azure.com/thutson79/TomsResumeCore/_build/latest?definitionId=3)

This repo is the code for my resume website, <a href="https://tomhutson.com" target="blank">tomhutson.com</a>. The goal here is to have a project to maintain to keep myself up to date on the latest technologies/practices and at the same time show that I am actually capable of doing the things listed on my resume. I realize it's not formatted for easy reuse but if anyone would like to fork it and use it for their own site, please feel free. 

## Features
A few things used in this site:
- .Net Core 3.0 Web project with .Net Standard class libraries for data, service, and business object layers. The web API is found in the web project. Normally I would seperate these but Azure sites aren't free. 
- Web project uses Razor Pages for the foundation, Sass files for styling, and Gulp/NPM to make life easier managing the script and styling tasks.
- Data is pulled from a Json file in Google Drive. I may move this into a more traditional storage mechanism but I like the idea of having an easily editable (and free !) storage piece for now.
- Vue is used to retrieve and display the work history from the web API.
- Unit tests using xUnit.
- Google ReCapthcha v2 is used to verify contact form submission.
- CI and hosting is done on Azure.
- SSL cert from <a href="https://www.cloudflare.com/" target="blank">Cloudflare</a>

## Credits
- Theme is modified from the <a href="https://github.com/BlackrockDigital/startbootstrap-agency" target="blank">Agency theme found at startbootstrap</a>.

## License
This project is licensed under the MIT License.
