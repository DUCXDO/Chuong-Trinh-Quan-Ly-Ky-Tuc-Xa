using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.DAO_class
{
    public interface IPhieuTrangBiDAO
    {
        PHIEUTRANGBI ThemPhieuTB(PHIEUTRANGBIDTO ptb);
        PHIEUTRANGBI XoaPhieuTB(String maTS, String maP);
        PHIEUTRANGBI SuaPhieuTB(PHIEUTRANGBIDTO ptb);
        IEnumerable<PHIEUTRANGBI> TimTatCaPhieuTrangBi();
        PHIEUTRANGBI TimPhieuTBTheoMaPhong(String maP);
        PHIEUTRANGBI TimPhieuTBTheoMaTaiSan(String maTS);
        IEnumerable<PHIEUTRANGBI> TimPhieuTB(PHIEUTRANGBIDTO ptb);
    }

    public class PhieuTrangBiDAO : IPhieuTrangBiDAO
    {
        public PHIEUTRANGBI ThemPhieuTB(PHIEUTRANGBIDTO ptb)
        {
            try
            {
                // khai báo và khởi tạo đối tượng kết nối với database
                KTXEntities KTXe = new KTXEntities();

                PHIEUTRANGBI add = new PHIEUTRANGBI();
                add.MaPhong = ptb.MaPhong;
                add.MaTS = ptb.MaTS;
                add.SoLuong = ptb.SoLuong;
                //Thêm mới 
                PHIEUTRANGBI result = KTXe.PHIEUTRANGBIs.Add(add);
                //Lưu thay đổi
                KTXe.SaveChanges();
                //trả về đối tượng mới thêm để xác định kết quả
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public PHIEUTRANGBI XoaPhieuTB(String maTS, String maP)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                PHIEUTRANGBI delete = KTXe.PHIEUTRANGBIs.SingleOrDefault(x => (x.MaPhong == maP) && (x.MaTS == maTS));
                PHIEUTRANGBI result = KTXe.PHIEUTRANGBIs.Remove(delete);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public PHIEUTRANGBI SuaPhieuTB(PHIEUTRANGBIDTO ptb)
        {
            try
            {
                KTXEntities KTXe = new KTXEntities();
                PHIEUTRANGBI edit = KTXe.PHIEUTRANGBIs.SingleOrDefault(x => x.MaPhong == ptb.MaPhong && x.MaTS == ptb.MaTS);
                edit.SoLuong = ptb.SoLuong;
                int result = KTXe.SaveChanges();
                if (result == 1)
                {
                    return edit;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public IEnumerable<PHIEUTRANGBI> TimTatCaPhieuTrangBi()
        {

            KTXEntities KTXe = new KTXEntities();
            IEnumerable<PHIEUTRANGBI> result = KTXe.PHIEUTRANGBIs.AsEnumerable();
            return result;

        }

        public PHIEUTRANGBI TimPhieuTBTheoMaPhong(String maP)
        {

            KTXEntities KTXe = new KTXEntities();
            PHIEUTRANGBI result = KTXe.PHIEUTRANGBIs.SingleOrDefault(x => x.MaPhong == maP);
            return result;

        }

        public PHIEUTRANGBI TimPhieuTBTheoMaTaiSan(String maTS)
        {

            KTXEntities KTXe = new KTXEntities();
            PHIEUTRANGBI result = KTXe.PHIEUTRANGBIs.SingleOrDefault(x => x.MaTS == maTS);
            return result;

        }

        public IEnumerable<PHIEUTRANGBI> TimPhieuTB(PHIEUTRANGBIDTO ptb)
        {

            KTXEntities KTXe = new KTXEntities();
            IEnumerable<PHIEUTRANGBI> result = KTXe.PHIEUTRANGBIs.AsQueryable().Where(x =>
            (ptb.MaTS == null || x.MaTS == ptb.MaTS) &&
            (ptb.MaPhong == null || x.MaPhong == ptb.MaPhong));
            return result;

        }
    }
}
