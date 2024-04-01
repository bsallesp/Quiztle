Create a list of three random source phrases and three random translations for each.
Do not include any explanations, only provide a  RFC8259 compliant JSON response  following this format without deviation.
[{
  "source_language": "language of original phrase",
  "source_phrase": "the phrase to be translated",
  "translations": [{
    "trans_language": "language of the translation",
    "translation": "the translated phrase"
  }]
}]
The JSON response:


Create a list of MIT computer science classes and the most relevant authors for each.
Do not include any explanations, only provide a  RFC8259 compliant JSON response  following this format without deviation.
[{
  "topics": [{
    "topic": {
      "topic name": "topic's name",
      "author": "author's name"
    }
  }]
}]

The JSON response:


On a scale from zero to 10, what is the likelihood that this is a name of a college, course, or something that can be used to build a study plan?

[here is the parameter]

Return a JSON with this structure:

{
  "probability": From 0 to 10,
  "is_college": true or false,
  "is_technical_course": true or false,
  "is_general_knowledge_content": true or false,
  "is_certification_program": true or false,
  "is_workshop_or_seminar": true or false,
  "is_online_course": true or false,
  "is_something_not_fitting_college_course_or_study_plan": true or false
}