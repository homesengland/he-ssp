using HE.Investments.Loans.Contract.Common;

namespace HE.Investments.Loans.Common.Tests.TestData;

public static class TextTestData
{
    public const string TextThatExceedsLongInputLimit =
        @"Morbi porta sit amet orci ac molestie. Duis efficitur felis ante, sit amet egestas enim suscipit vel. Curabitur dignissim orci nisl, ac vulputate enim scelerisque ut. Maecenas quis erat facilisis, porta nunc vel, interdum turpis. Etiam aliquam ac eros nec scelerisque. Suspendisse eu ultricies sapien. Vivamus viverra erat eu leo laoreet, et scelerisque quam imperdiet. Sed a felis sapien. Cras vitae viverra velit. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Etiam vel magna sapien. Maecenas tincidunt auctor turpis eu faucibus.
                                              Ut dapibus commodo enim, non tincidunt erat pulvinar vitae. Etiam pellentesque sem neque, a posuere felis porta id. Phasellus egestas lacus sit amet ipsum consectetur, ut fringilla justo lacinia. Donec mollis purus ac ullamcorper sagittis. Praesent fringilla tristique tortor vitae vulputate. Curabitur accumsan convallis sollicitudin. Vestibulum est est, placerat id lorem nec, elementum consectetur ipsum. Integer interdum elit purus, in accumsan enim sagittis quis. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Nulla scelerisque nec ligula in auctor. Integer facilisis, turpis non fermentum venenatis, augue sapien placerat lorem, id volutpat magna risus ut risus. Morbi pharetra suscipit dictum. Nulla id ante eu sem finibus rutrum. Sed dui elit, tincidunt nec lobortis et, sollicitudin sit amet mi. Donec tempor neque nequerx. Quisque sollicitudin, eros non semper elementum, quam magna tincidunt diam, vitae interdum magna purus ut lorem.";

    public const string TextThatNotExceedsLongInputLimit =
        "ras vitae pellentesque nisl. Pellentesque tempor, ex eu finibus lobortis, dolor odio pellentesque dui, id fringilla mauris mauris sollicitudin nunc. Quisque sollicitudin, eros non semper elementum, quam magna tincidunt diam, vitae interdum magna purus ut lorem. Nam egestas augue sit amet tortor eleifend commodo. Morbi dapibus et velit in porttitor. Curabitur egestas ipsum libero, vel scelerisque tortor sollicitudin sit amet. Curabitur quis convallis velit. Etiam vel ornare mi. Nulla sem justo, rhoncus eu volutpat a, luctus quis felis. Donec malesuada lorem vitae finibus maximus. Nam tempus tempus convallis. Phasellus scelerisque gravida diam a iaculis. Nulla vehicula tristique vehicula. Interdum et malesuada fames ac ante ipsum primis in faucibus. Integer at nisl quam. Curabitur venenatis ligula sapien, nec lobortis nisi pellentesque quis. Ut molestie augue a quam porttitor, et faucibus orci vestibulum. Quisque quis ante vestibulum, feugiat dui sed, porta odio. Vivamus ac ex a laoreet. Pellentesque tempor, ex eu finibus lobortis, dolor odio pellentesque dui, id fringilla mauris mauris sollicitudin nunc. Pellentesque tempor, ex eu finibus lobortis, dolor odio pellentesque dui, id fringilla mauris mauris sollicitudin nunc. Carq was here. Quisque quis ante vestibulum, feugiat dui sed, porta odio. Vivamus ac ex a laoreet. Pellentesque tempor, ex eu finibus lobortis, dolor odio pellentesque dui, id fringilla mauris mauris sollicitudin nunc. Pellentesque tempor, ex eu finibus lob.";

    public const string TextThatExceedsShortInputLimit = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis fermentum efficitur condimentum libero. Quisque sollicitudin.";

    public const string TextThatNotExceedsShortInputLimit = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis fermentum efficitur condimentum.";

    public static readonly ShortText CorrectShortText = new("text");

    public static readonly LongText CorrectLongText = new("text");
}
