FROM python

WORKDIR /app

COPY apply_changes.py /app/apply_changes.py

COPY Dordrecht_Merged_XSD_Validated.xml /app/Dordrecht_Merged_XSD_Validated.xml

RUN mkdir /app/output

CMD python apply_changes.py
