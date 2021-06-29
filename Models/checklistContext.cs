using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CheckList.Models
{
    public partial class checklistContext : DbContext
    {
        public checklistContext()
        {
        }

        public checklistContext(DbContextOptions<checklistContext> options)
            : base(options)
        {
        }

        public virtual DbSet<cartella> cartella { get; set; }
        public virtual DbSet<checklist> checklist { get; set; }
        public virtual DbSet<domande> domande { get; set; }
        public virtual DbSet<dottori> dottori { get; set; }
        public virtual DbSet<fase> fase { get; set; }
        public virtual DbSet<infermieri> infermieri { get; set; }
        public virtual DbSet<noconformita> noconformita { get; set; }
        public virtual DbSet<paziente> paziente { get; set; }
        public virtual DbSet<percorsoassistenziale> percorsoassistenziale { get; set; }
        public virtual DbSet<reparto> reparto { get; set; }
        public virtual DbSet<riepilogo> riepilogo { get; set; }
        public virtual DbSet<unitaoperativa> unitaoperativa { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=checklist;user=sa;password=root;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<cartella>(entity =>
            {
                entity.Property(e => e.dataPianificata).HasColumnType("date");

                entity.Property(e => e.diagnosi)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.indicazioniRicovero)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.percorsoAssistenziale)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.unitaOperativa)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.idPazienteNavigation)
                    .WithMany(p => p.cartella)
                    .HasForeignKey(d => d.idPaziente)
                    .HasConstraintName("FK_cartella_paziente");
            });

            modelBuilder.Entity<checklist>(entity =>
            {
                entity.Property(e => e.data).HasColumnType("datetime");

                entity.Property(e => e.diagnosi)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.percorso)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.signinEnd).HasColumnType("datetime");

                entity.Property(e => e.signinInit).HasColumnType("datetime");

                entity.Property(e => e.signoutEnd).HasColumnType("datetime");

                entity.Property(e => e.signoutInit).HasColumnType("datetime");

                entity.Property(e => e.timeoutEnd).HasColumnType("datetime");

                entity.Property(e => e.timeoutInit).HasColumnType("datetime");

                entity.HasOne(d => d.idPazienteNavigation)
                    .WithMany(p => p.checklist)
                    .HasForeignKey(d => d.idPaziente)
                    .HasConstraintName("FK_checklist_paziente");

                entity.HasOne(d => d.idRepartoNavigation)
                    .WithMany(p => p.checklist)
                    .HasForeignKey(d => d.idReparto)
                    .HasConstraintName("FK_checklist_reparto");
            });

            modelBuilder.Entity<domande>(entity =>
            {
                entity.Property(e => e.domanda)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.idFaseNavigation)
                    .WithMany(p => p.domande)
                    .HasForeignKey(d => d.idFase)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_domande_fase");
            });

            modelBuilder.Entity<dottori>(entity =>
            {
                entity.Property(e => e.cognome)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.nome)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<fase>(entity =>
            {
                entity.Property(e => e.nome)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<infermieri>(entity =>
            {
                entity.Property(e => e.cognome)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.nome)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<noconformita>(entity =>
            {
                entity.Property(e => e.id).ValueGeneratedNever();

                entity.Property(e => e.testo)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.idDomandeNavigation)
                    .WithMany(p => p.noconformita)
                    .HasForeignKey(d => d.idDomande)
                    .HasConstraintName("FK_noconformita_domande");
            });

            modelBuilder.Entity<paziente>(entity =>
            {
                entity.Property(e => e.codiceFiscale)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.cognome)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.dataNascita).HasColumnType("date");

                entity.Property(e => e.nome)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<percorsoassistenziale>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.percorso)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<reparto>(entity =>
            {
                entity.Property(e => e.id).ValueGeneratedNever();

                entity.Property(e => e.nome)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<riepilogo>(entity =>
            {
                entity.Property(e => e.risposta)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.idChecklistNavigation)
                    .WithMany(p => p.riepilogo)
                    .HasForeignKey(d => d.idChecklist)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_riepilogo_checklist");

                entity.HasOne(d => d.idDomandeNavigation)
                    .WithMany(p => p.riepilogo)
                    .HasForeignKey(d => d.idDomande)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_riepilogo_domande");

                entity.HasOne(d => d.idNoconformitaNavigation)
                    .WithMany(p => p.riepilogo)
                    .HasForeignKey(d => d.idNoconformita)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_riepilogo_noconformita");
            });

            modelBuilder.Entity<unitaoperativa>(entity =>
            {
                entity.Property(e => e.id).ValueGeneratedNever();

                entity.Property(e => e.unita)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
