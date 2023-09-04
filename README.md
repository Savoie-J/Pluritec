# Pluritec Layer to Layer

TLDR; This project requires Docker Desktop Linux Containers to run locally as its original program relied on production databases. 

  - Run 'docker-compose up -d' in the powershell console to start this container prior to running the project, and then run 'docker-compose down' once you are finished to terminate the container.



This project's goal was to act as a single website for engineers and operators alike working with a Pluritec X-Ray machine measuring alignment data for printed circuit boards.

As such it has two sections, one for operators, and one for engineers. The default view shows files output by the machine and allows for log entries to be made by operators with these files. 

Upon submission of these files, the website utilizes the job number scanned by operators to query the production database and finds the associated part number. Once found it searches for a specification by which we should use to analyze the alignment data.

The second section of this project is the several specification views for engineers, those being for defined entries and undefined entries. Once a log entry is made by operators, if a specification is not found, that entry is kept on the undefined page, waiting for an engineer to create
a specification for it, otherwise if an entry was found, it is automatically analyzed by those parameters.

On the defined entries page, engineers can then click the 'Logs' button alongside each part number to see all logs made by operators, with color coding on their buttons to indicate whether the alignment data was in specification or not, alongside email alerts informing them of such.
