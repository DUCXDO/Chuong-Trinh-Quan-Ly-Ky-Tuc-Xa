using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace DAO
{   public interface IPhieuGhiDienNuocDAO
    {
        PHIEUGHIDIENNUOC ThemSGDN(PHIEUGHIDIENNUOCDTO pg);
        PHIEUGHIDIENNUOC SuaPGDN(PHIEUGHIDIENNUOCDTO pg);
        PHIEUGHIDIENNUOC XoaPGDN(String MaPG);
        IEnumerable<PHIEUGHIDIENNUOC> TimTatCaPGDN();
        IEnumerable<PHIEUGHIDIENNUOC> TimPGDNTheoLoaiPGDN(bool LoaiPG);
        IEnumerable<PHIEUGHIDIENNUOC> TimSGDN(PHIEUGHIDIENNUOC pg);
    }
    class PhieuGhiDienNuocDAO : IPhieuGhiDienNuocDAO
    {
        public PHIEUGHIDIENNUOC ThemSGDN(PHIEUGHIDIENNUOCDTO pg)
        {
            try
            {
                //Khai báo và khởi tạo đối tượng kết nối database
                KTXEntities KTXe = new KTXEntities();
                PHIEUGHIDIENNUOC addPG = new PHIEUGHIDIENNUOC();
                addPG.LoaiPhieuGhi = pg.LoaiPhieuGhi;
                addPG.MaPhong = pg.MaPhong;
                addPG.MaSo = pg.MaSo;
                addPG.NgayGhi = pg.NgayGhi;
                addPG.SoDienNuoc = pg.SoDienNuoc;
                //Thêm SG mới
                PHIEUGHIDIENNUOC result = KTXe.PHIEUGHIDIENNUOCs.Add(addPG);
                //Lưu thay đổi
                KTXe.SaveChanges();
                //Trả về đối tượng mới thêm
                return result;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public PHIEUGHIDIENNUOC SuaPGDN(PHIEUGHIDIENNUOCDTO pg)
        {
            try
            {
                //Khai báo kết nối data
                KTXEntities KTXe = new KTXEntities();
                PHIEUGHIDIENNUOC editPG = KTXe.PHIEUGHIDIENNUOCs.SingleOrDefault(x => x.MaPhong == pg.MaPhong);
                editPG.LoaiPhieuGhi = pg.LoaiPhieuGhi;
                editPG.MaPhong = pg.MaPhong;
                editPG.MaSo = pg.MaSo;
                editPG.NgayGhi = pg.NgayGhi;
                editPG.SoDienNuoc = pg.SoDienNuoc;
                int result = KTXe.SaveChanges();
                if (result == 1)
                {
                    return editPG;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public PHIEUGHIDIENNUOC XoaPGDN(String MaPG)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                PHIEUGHIDIENNUOC delPG = KTXe.PHIEUGHIDIENNUOCs.Find(MaPG);
                PHIEUGHIDIENNUOC result = KTXe.PHIEUGHIDIENNUOCs.Remove(delPG);
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<PHIEUGHIDIENNUOC> TimTatCaPGDN()
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<PHIEUGHIDIENNUOC> result = KTXe.PHIEUGHIDIENNUOCs.AsEnumerable();
            return result;
        }
        public IEnumerable <PHIEUGHIDIENNUOC> TimPGDNTheoLoaiPGDN(bool LoaiPG)
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<PHIEUGHIDIENNUOC> findPG = KTXe.PHIEUGHIDIENNUOCs.AsQueryable().Where(x => x.LoaiPhieuGhi == LoaiPG);
            return findPG;

        }
        public IEnumerable<PHIEUGHIDIENNUOC> TimSGDN(PHIEUGHIDIENNUOC pg)
        {
            KTXEntities KTXe = new KTXEntities();
            IEnumerable<PHIEUGHIDIENNUOC> findPG = KTXe.PHIEUGHIDIENNUOCs.AsQueryable().Where(x => (pg.LoaiPhieuGhi == null || x.LoaiPhieuGhi == pg.LoaiPhieuGhi) && (pg.MaPhong == null || x.MaPhong == pg.MaPhong) && (pg.MaSo == null || x.MaSo == pg.MaSo)&&(pg.NgayGhi==null|| x.NgayGhi== pg.NgayGhi)&&(pg.SoDienNuoc==null|| x.SoDienNuoc== pg.SoDienNuoc));
            return findPG;
        }
    }
}

