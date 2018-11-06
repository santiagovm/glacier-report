using System;
using System.Collections.Generic;

namespace Glacier.Tools.Entities
{
    public class VaultInventory
    {
        public VaultInventory(string vaultArn, DateTimeOffset inventoryDate, IEnumerable<GlacierFile> archiveList)
        {
            VaultArn = vaultArn;
            InventoryDate = inventoryDate;
            ArchiveList = archiveList;
        }

        public string VaultArn { get; private set; }

        public DateTimeOffset InventoryDate { get; private set; }

        public IEnumerable<GlacierFile> ArchiveList { get; private set; }
    }
}
