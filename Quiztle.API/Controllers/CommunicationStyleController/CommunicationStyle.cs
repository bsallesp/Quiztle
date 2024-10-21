using System;
using System.Collections.Generic;

public class CommunicationStyle
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Groups { get; set; }

    public static List<CommunicationStyle> items = new List<CommunicationStyle>
    {
        new CommunicationStyle
        {
            Id = new Guid("d1f60b8e-4c9a-4f2e-86b2-e3403fc0e93b"),
            Title = "Formality Level",
            Description = "Degree of formality in language.",
            Groups = new[] { "questions-certifications", "linkedin-post", "instagram-post" }
        },
        new CommunicationStyle
        {
            Id = new Guid("5f3be7ab-7d6b-46e6-90d4-ade5dfb3f631"),
            Title = "Tone of Voice",
            Description = "Overall attitude or emotion conveyed through words.",
            Groups = new[] { "questions-certifications", "linkedin-post", "instagram-comment" }
        },
        new CommunicationStyle
        {
            Id = new Guid("af7f0a5f-542f-4e9c-985c-3f79b19a78e8"),
            Title = "Prosody and Rhythm",
            Description = "Patterns of stress and intonation in speech.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("67b3d685-2f3a-4f2f-93b5-c71d2e11fa52"),
            Title = "Detail Level",
            Description = "Amount of specificity and elaboration in communication.",
            Groups = new[] { "questions-certifications", "linkedin-post" }
        },
        new CommunicationStyle
        {
            Id = new Guid("3f1a1450-d160-490e-8c8a-d0acfa8f1e31"),
            Title = "Coherence and Organization",
            Description = "Logical flow and structure of ideas.",
            Groups = new[] { "questions-certifications", "linkedin-post" }
        },
        new CommunicationStyle
        {
            Id = new Guid("db00ac2f-7054-474e-9f76-8e8435487db2"),
            Title = "Empathy and Sensitivity",
            Description = "Ability to connect with the audience emotionally.",
            Groups = new[] { "questions-certifications", "linkedin-post", "instagram-comment" }
        },
        new CommunicationStyle
        {
            Id = new Guid("d67c47f1-ef7c-4b67-b48b-c54c0b6c8e58"),
            Title = "Positivity/Negativity",
            Description = "General sentiment or attitude expressed.",
            Groups = new[] { "questions-certifications", "linkedin-post" }
        },
        new CommunicationStyle
        {
            Id = new Guid("b1d4b3d7-5bfa-42c8-8970-bdb314e5c289"),
            Title = "Directness and Objectivity",
            Description = "Clarity and straightforwardness of the message.",
            Groups = new[] { "questions-certifications", "linkedin-post" }
        },
        new CommunicationStyle
        {
            Id = new Guid("3bcab37d-498c-4fd4-8bc5-7689e5c00766"),
            Title = "Punctuation and Pauses",
            Description = "Use of punctuation marks and breaks for emphasis.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("b29b5cb2-e8d4-4c11-89bb-402a7d9b8458"),
            Title = "Emotional Intensity",
            Description = "Strength of emotions conveyed in the message.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("c69d3965-7195-4e0e-bb60-e4c95142a9d1"),
            Title = "Originality and Creativity",
            Description = "Uniqueness of ideas and expression.",
            Groups = new[] { "questions-certifications", "linkedin-post", "instagram-post" }
        },
        new CommunicationStyle
        {
            Id = new Guid("27a21f0b-ec4d-4f8d-b1bc-5037dc6a00cd"),
            Title = "Explicitness Level",
            Description = "Clarity and straightforwardness of information.",
            Groups = new[] { "questions-certifications", "linkedin-post" }
        },
        new CommunicationStyle
        {
            Id = new Guid("653b7a69-d190-45e4-9e9a-e2799f325ac4"),
            Title = "Frequency of Quotes or Data",
            Description = "Use of external sources to support arguments.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("9b5c9350-d82b-4c87-802b-7ff925633433"),
            Title = "Repetition of Key Concepts",
            Description = "Recurrence of important ideas for emphasis.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("50ed77cb-43b6-41c2-bb04-f75e7a66f5c4"),
            Title = "Irony and Sarcasm",
            Description = "Use of figures of speech that convey a meaning opposite to the literal one.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("f32510b3-7db8-42b0-9ac8-3385c61146af"),
            Title = "Cultural Context Adaptation",
            Description = "Tailoring content to fit the cultural background of the audience.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("d3e21f71-5d75-4fa8-8350-6c6a54f9ed8e"),
            Title = "Preferred Vocabulary",
            Description = "Specific words or phrases favored in communication.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("47441805-2581-4e29-8f88-dfa08604e4b1"),
            Title = "Writing Style",
            Description = "Overall approach to structuring sentences and paragraphs.",
            Groups = new[] { "questions-certifications", "linkedin-post", "instagram-post", "instagram-comment" }
        },
        new CommunicationStyle
        {
            Id = new Guid("f5b37787-b7c1-4780-8458-bd07de8d5bb2"),
            Title = "Use of Connectors",
            Description = "Transitional words and phrases that link ideas.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("4b1411a0-cb38-4db5-b8c1-161a0c6a25bc"),
            Title = "Preferred Syntax",
            Description = "Specific sentence structures used.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("1e4919be-92e8-4820-b013-05dc04c7c711"),
            Title = "Use of Figurative Language",
            Description = "Incorporation of metaphors, similes, etc.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("ae0bc8b0-9474-4739-9f3f-3280d6192b2a"),
            Title = "Use of Emojis and Graphics",
            Description = "Inclusion of visual elements in written communication.",
            Groups = new[] { "questions-certifications", "instagram-post", "instagram-comment" }
        },
        new CommunicationStyle
        {
            Id = new Guid("f047b0b1-2c66-40a4-bf78-d7e04e9db52c"),
            Title = "Point of View",
            Description = "Perspective from which the content is conveyed (first, second, third person).",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("2d20ef13-1664-4670-9f36-367a5f7ef6c3"),
            Title = "Cultural References Usage",
            Description = "Incorporation of references familiar to the target audience.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("74679b65-f7a5-49cf-b7de-b98c48baf16c"),
            Title = "Linguistic Register",
            Description = "Level of formality or informality in language.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("6a92d070-7d81-4263-92d2-b720cfbfeb95"),
            Title = "Use of Jargon and Technical Terms",
            Description = "Specialized vocabulary related to a specific field.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("34cbd134-e01c-44bc-b5b4-9a9147be8a8e"),
            Title = "Narrative Structure",
            Description = "Organization of content in a storytelling format.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("03cf9f00-2a4c-493c-bde0-e6fc55e5838d"),
            Title = "Frequency of Rhetorical Questions",
            Description = "Use of questions designed to make a point rather than elicit an answer.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("c4e1c13a-4600-4f00-b5b1-9274efb2e15d"),
            Title = "Use of Examples and Illustrations",
            Description = "Providing concrete instances to clarify points.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("d037e645-9d0a-4de4-8c1e-5bc86fbb33e5"),
            Title = "Audience Adaptation",
            Description = "Adjusting content based on the characteristics and preferences of the audience.",
            Groups = new[] { "questions-certifications" }
        },
        new CommunicationStyle
        {
            Id = new Guid("77d7010a-33ab-4764-8889-fb4d597b495c"),
            Title = "Clarity and Conciseness",
            Description = "The straightforwardness of communication without unnecessary words.",
            Groups = new[] { "questions-certifications", "linkedin-post" }
        },
        new CommunicationStyle
        {
            Id = new Guid("f7dc7f57-abe5-4290-8c57-353e693730f2"),
            Title = "Humor and Wit",
            Description = "Use of humor to engage the audience.",
            Groups = new[] { "questions-certifications", "instagram-comment" }
        }
    };
}
