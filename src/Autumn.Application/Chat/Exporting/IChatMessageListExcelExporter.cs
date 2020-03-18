using System.Collections.Generic;
using Autumn.Chat.Dto;
using Autumn.Dto;

namespace Autumn.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(List<ChatMessageExportDto> messages);
    }
}
