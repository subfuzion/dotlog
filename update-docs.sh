#!/bin/sh
curl -X POST --data-urlencode content@README.md http://documentup.com/compiled > gendocs.html && open gendocs.html
