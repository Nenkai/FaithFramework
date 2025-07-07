using FF16Framework.Interfaces.ImGuiManager;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.ImGuiManager;

public class ImGuiImageState : IQueuedImGuiImage
{
    public bool IsLoaded { get; set; }
    public IImGuiImage? Image { get; set; }

    public void Dispose()
    {
         Image?.Dispose();
    }
}
